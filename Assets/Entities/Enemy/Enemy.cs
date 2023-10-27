using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource), typeof(Animator))]
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
    [SerializeField] float awarenessRange;

    [Header("Attacking")]
    [Tooltip("Amount of attacks pr. second")]
    [SerializeField] float attackSpeed;
    [SerializeField] float attackDamage;

    float attackCooldown = 0;

    EnemyState state;
    NavMeshQueryFilter filter;
    NavMeshAgent agent;
    AudioSource stompSFX;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stompSFX= GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        filter.agentTypeID = agent.agentTypeID;
        filter.areaMask = NavMesh.AllAreas;

        GameManager.Instance.OnNoise += ListenForNoise;
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
        attackCooldown = Mathf.Max(attackCooldown, 0);
        if (state == EnemyState.Attacking)
        {
            animator.SetBool("IsAttacking", true);
            if (attackCooldown == 0)
            {
                SanityManager.Instance.AttackSanity(attackDamage);
                attackCooldown = 1 / attackSpeed;
            }

            return;
        }

        else if (TryHuntTarget())
        {
            state = EnemyState.Chasing;
            animator.SetBool("IsRunning", true);
        }

        else if (agent.remainingDistance <= agent.stoppingDistance)
            state = EnemyState.FinishedRoute;

        else if(state != EnemyState.Chasing)
        {
            state = EnemyState.Scouting;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsRunning", false);
        }

        if (state == EnemyState.FinishedRoute)
        {
            agent.speed = wanderSpeed;
            agent.acceleration = wanderAcc;
            if (!FindRandomPath(0f, 50))
                FindRandomPath(180f, 50);
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger("OnDamage");
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

        // Draw range where the enemy can see the player everywhere.
        Handles.DrawWireDisc(transform.position, transform.up, awarenessRange, 2);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("ATTACK");
        if (collision.GetComponent<Collider>().CompareTag("Player"))
            state = EnemyState.Attacking;
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("RETREAT");
        if (collision.GetComponent<Collider>().CompareTag("Player"))
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

        agent.speed = huntingSpeed;
        agent.acceleration = huntingAcc;

        animator.SetBool("IsRunning", true);

        state = EnemyState.Chasing;
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
        if ((target.position - transform.position).magnitude < awarenessRange)
        {
            agent.speed = huntingSpeed;
            agent.acceleration = huntingAcc;

            agent.SetDestination(target.position);
            return true;
        }

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

            agent.speed = huntingSpeed;
            agent.acceleration = huntingAcc;

            agent.SetDestination(hit.point);
            return true;
        }

        return false;
    }

    // temporary, probably good enough but can be replaced with animation based timing
    IEnumerator MakeStompSound(float stompPrSpeed)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / (stompPrSpeed * agent.speed));

            stompSFX.Play();
        }
    }

}

enum EnemyState
{
    FinishedRoute,
    Scouting,
    Chasing,
    Attacking
}