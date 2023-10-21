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

    [SerializeField]
    private GameObject patroler;

    private NavMeshAgent ghostAgent;
    private NavMeshAgent patrolAgent;

    public Int32 initialDir;
    public Int32 wpIndex;

    public GameObject[] waypoints;
    

    void Start()
    {
        ghost = this.gameObject;
        ghostAgent = ghost.GetComponent<NavMeshAgent>();
        patrolAgent = patroler.GetComponent<NavMeshAgent>();

        initialDir = UnityEngine.Random.Range(0, 2);
        wpIndex = UnityEngine.Random.Range(0, waypoints.Length);

        ghost.transform.position = waypoints[wpIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ghostAgent.pathPending && ghostAgent.remainingDistance < 0.5f)
        {
            wpIndex = AI.Movement.FollowPatrolPath(initialDir, wpIndex, waypoints, ghostAgent);
        }

        AI.Movement.Seek(patrolAgent, ghost.transform);
    }
}