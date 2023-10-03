using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    private GameObject ghost;

    public NavMeshAgent agent;

    public Int32 randomWP;
    public Int32 randomDir;

    public int patrolWP = 0;
    public GameObject[] waypoints;
    

    void Start()
    {
        ghost = this.gameObject;
        agent = ghost.GetComponent<NavMeshAgent>();

        randomWP = UnityEngine.Random.Range(0, waypoints.Length);
        patrolWP = randomWP;
        ghost.transform.position = waypoints[randomWP].transform.position;

        randomDir = UnityEngine.Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) FollowPath();
    }

    void FollowPath()
    {
        if(randomDir % 2 == 0)
            patrolWP = (patrolWP + 1) % waypoints.Length;
        else
        {
            if(patrolWP <= 0)
                patrolWP = waypoints.Length;

            patrolWP = (patrolWP - 1) % waypoints.Length;
        }
            

        Seek();
    }

    void Seek()
    {
        agent.destination = waypoints[patrolWP].transform.position;
    }
}