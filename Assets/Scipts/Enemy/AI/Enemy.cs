using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    
    [Header("Speed")]
    [SerializeField] float huntingSpeed;
    [SerializeField] float wanderSpeed;
    [SerializeField] float huntingAcc;
    [SerializeField] float wanderAcc;

    [Header("Wandering")]
    [SerializeField] float wanderMaxTurnAngle;
    [SerializeField] float wanderMaxRange;
    [SerializeField] float wanderMinRange;
    [SerializeField] float viewRange;

    NavMeshQueryFilter filter;
    NavMeshAgent agent;
    bool attacking;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        filter.agentTypeID = agent.agentTypeID;
        filter.areaMask = NavMesh.AllAreas;
        StartCoroutine(ScanForPrey());
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.speed = wanderSpeed;
            agent.acceleration = wanderAcc;
            if (!FindRandomPath(0f, 50))
                FindRandomPath(180f, 50);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            attacking = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            attacking = false;
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


    IEnumerator ScanForPrey()
    {
        RaycastHit hit;

        while (true)
        {
            // Keep updating destination while in view
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, viewRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    agent.speed = huntingSpeed;
                    agent.acceleration = huntingAcc;

                    agent.SetDestination(hit.point);
                    yield return new WaitForFixedUpdate();
                    continue;
                }
            }

            // Longer update times when out of view
            yield return new WaitForSeconds(1);
        }
    }
}