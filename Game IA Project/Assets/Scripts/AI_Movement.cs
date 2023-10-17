using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
    public void Seek(NavMeshAgent _agent, Transform _target)
    {
        _agent.destination = _target.transform.position;
    }

    public IEnumerator Wander(float wanderRadius, float wanderTimer, NavMeshAgent _agent)
    {
        // Pick a random point within the wanderRadius
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

        // Set the NavMeshAgent's destination to the new position
        _agent.SetDestination(newPos);

        // Wait for a short time before choosing a new destination
        yield return new WaitForSeconds(wanderTimer);
    }

    // Generate a random point within a sphere (for wandering)
    public Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}