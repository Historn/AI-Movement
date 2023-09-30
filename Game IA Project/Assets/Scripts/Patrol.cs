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

    public GameObject[] waypoints;
    int patrolWP = 0;

    void Start()
    {
        ghost = this.gameObject;
        agent = ghost.GetComponent<NavMeshAgent>();
        randomWP = UnityEngine.Random.Range(0, waypoints.Length);
        patrolWP = randomWP;
        ghost.transform.position = waypoints[randomWP].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) FollowPath();
    }

    void FollowPath()
    {
        patrolWP = (patrolWP + 1) % waypoints.Length;
        Seek();
    }

    void Seek()
    {
        agent.destination = waypoints[patrolWP].transform.position;
    }
}