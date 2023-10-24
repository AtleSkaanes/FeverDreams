using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    
    [Header("Speed")]
    [SerializeField] float huntingSpeed;
    [SerializeField] float huntingAcc;
    [SerializeField] float wanderSpeed;
    [SerializeField] float wanderAcc;

    [Header("Wandering")]
    [SerializeField] float wanderMaxTurnAngle;
    [SerializeField] float wanderMaxRange;
    [SerializeField] float wanderMinRange;

    [Header("Scouting")]
    [SerializeField] float viewRange;
    [SerializeField] float viewAngle;
    [SerializeField] float hearingRange;

    EnemyState state;
    NavMeshQueryFilter filter;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        filter.agentTypeID = agent.agentTypeID;
        filter.areaMask = NavMesh.AllAreas;

        GameManager.Instance.OnNoise += ListenForNoise;
    }

    void Update()
    {
        if (state == EnemyState.Attacking)
        {
            // ATTAAACKK!!!
            return;
        }

        if (TryHuntTarget())
            state = EnemyState.Chasing;

        else if (agent.remainingDistance <= agent.stoppingDistance)
            state = EnemyState.FinishedRoute;

        else
            state = EnemyState.Scouting;


        // Enemy is not done hunting or walking to next random point
        if (state == EnemyState.FinishedRoute)
        {
            agent.speed = wanderSpeed;
            agent.acceleration = wanderAcc;
            if (!FindRandomPath(0f, 50))
                FindRandomPath(180f, 50);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw FOV
        for (int i = -1; i < 2; i++)
        {
            Vector3 lineDir = Quaternion.AngleAxis(viewAngle * i, Vector3.up) * transform.forward;
            Vector3 endPos = transform.position + lineDir * viewRange;

            Gizmos.DrawLine(transform.position, endPos);
        }

        // Draw hearing Range
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, hearingRange, 3);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) { }
            state = EnemyState.Attacking;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            state = EnemyState.FinishedRoute;
    }

    void ListenForNoise(Vector3 position)
    {
        if (state > EnemyState.Scouting)
            return;

        float distance = (position - transform.position).magnitude;
        if (distance > hearingRange)
            return;

        agent.SetDestination(position);
        state = EnemyState.Chasing;
    }

    void UpdateState()
    {
        
    }

    bool FindRandomPath(float angleOffset, int tries)
    {
        NavMeshHit hit;
        for (int i = 0; i < tries; i++)
        {
            float randomAngle = Random.Range(-wanderMaxTurnAngle, wanderMaxTurnAngle);
            float randomDistance = Random.Range(wanderMinRange, wanderMaxRange);
            Vector3 randomDirection = Quaternion.AngleAxis(randomAngle + angleOffset, Vector3.up) * (transform.forward * randomDistance);

            if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, agent.height * 2f, filter))
            {
                agent.SetDestination(hit.position);
                return true;
            }
        }

        return false;
    }

    bool TryHuntTarget()
    {
        // Angle between 2 vectors:
        //                a ● b
        // angle = ACos( ─────── )
        //               |a|*|b|

        Vector3 targetDir = (target.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, targetDir);

        // Due to floating point precision
        dotProduct = Mathf.Clamp(dotProduct, -1, 1);

        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        if (angle > viewAngle)
            return false;


        RaycastHit hit;
        // Keep updating destination while in view
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, viewRange))
        {
            if (!hit.collider.CompareTag("Player"))
                return false;

            state = EnemyState.Chasing;
            agent.speed = huntingSpeed;
            agent.acceleration = huntingAcc;

            agent.SetDestination(hit.point);
            return true;
        }

        return false;
    }
}

enum EnemyState
{
    FinishedRoute,
    Scouting,
    Chasing,
    Attacking
}