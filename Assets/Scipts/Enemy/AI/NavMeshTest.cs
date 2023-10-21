using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Transform target;
    [SerializeField] float viewRange;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(UpdateVision());
    }

    IEnumerator UpdateVision()
    {
        RaycastHit hit;
        while (true)
        {
            // Keep updating destination while in view
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, viewRange))
            {
                agent.SetDestination(hit.point);
                yield return new WaitForFixedUpdate();
                continue;
            }

            // Longer update times when out of view
            yield return new WaitForSeconds(1);
        }
    }
}
