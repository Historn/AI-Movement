using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class AI_Movement
{
    public static void Seek(NavMeshAgent agent, Transform target)
    {
        agent.destination = target.transform.position;
    }

    public static IEnumerator Wander(float wanderRadius, float wanderTimer, NavMeshAgent agent)
    {
        Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        yield return new WaitForSeconds(wanderTimer);
    }

    // Generate a random point within a sphere (for wandering)
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    public static Int32 FollowPatrolPath(Int32 initialDir, Int32 wpIndex, GameObject[] waypoints, NavMeshAgent agent)
    {
        if (initialDir == 0)
            wpIndex = (wpIndex + 1) % waypoints.Length;
        else
        {
            if (wpIndex <= 0)
                wpIndex = waypoints.Length;

            wpIndex = (wpIndex - 1) % waypoints.Length;
        }

        agent.destination = waypoints[wpIndex].transform.position;

        return wpIndex;
    }
}