using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WanderingBehavior : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private Transform target;       
    private NavMeshAgent agent;     

    private float timer;            

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // Start the wandering behavior
        StartCoroutine(Wander());
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to choose a new wander destination
        if (timer >= wanderTimer)
        {
            StartCoroutine(Wander());
            timer = 0;
        }
    }

    IEnumerator Wander()
    {
        // Pick a random point within the wanderRadius
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

        // Set the NavMeshAgent's destination to the new position
        agent.SetDestination(newPos);

        // Wait for a short time before choosing a new destination
        yield return new WaitForSeconds(wanderTimer);
    }

    // Generate a random point within a sphere (for wandering)
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}