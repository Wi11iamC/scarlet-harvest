using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PigBehaviorNameSpace {
public class PigBehavior : MonoBehaviour
{
    public float roamRadius = 10f; // The radius within which the pig will roam
    public float roamTime = 5f; // The time the pig will spend roaming in one direction
    public float speed = 2f; // The speed at which the pig will move
    public float turnSpeed = 5f; // The speed at which the pig will turn

    private Vector3 targetPosition; // The position the pig is currently moving towards
    private float timeSinceRoamChange; // The time since the pig last changed direction while roaming
    private NavMeshAgent navMeshAgent; // Reference to the pig's NavMeshAgent component

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        timeSinceRoamChange = 0f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = turnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Roam around
        if (timeSinceRoamChange >= roamTime)
        {
            // Pick a new random target position within the roam radius
            Vector2 randomDirection = Random.insideUnitCircle;
            Vector3 newTargetPosition = transform.position + new Vector3(randomDirection.x, 0f, randomDirection.y) * roamRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newTargetPosition, out hit, roamRadius, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
            }
            timeSinceRoamChange = 0f;
        }
        else
        {
            timeSinceRoamChange += Time.deltaTime;
        }

        // Move towards the target position
        navMeshAgent.SetDestination(targetPosition);
    }
}
}