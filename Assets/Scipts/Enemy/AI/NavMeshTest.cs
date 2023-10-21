using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float huntingSpeed;
    [SerializeField] float wanderSpeed;
    [SerializeField] float huntingAcc;
    [SerializeField] float wanderAcc;
    
    [Space(15)]
    [Header("Misc.")]
    [SerializeField] float viewRange;
    [SerializeField] float wanderDistance;
    [SerializeField] Transform target;


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
        StartCoroutine(ScanForPrey());
    }

    private void Update()
    {
        if (Vector3.Distance(agent.destination, transform.position) < 2f)
        {
            print("Hello");

            agent.speed = wanderSpeed;
            agent.acceleration = wanderAcc;

            Vector3 randomDirection = Random.insideUnitSphere * wanderDistance;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderDistance, filter))
            {
                agent.SetDestination(hit.position);
                Debug.Log($"Set position. Distance is : {Vector3.Distance(agent.destination, transform.position)}");
            }
        }

        print($"{agent.destination}");
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
