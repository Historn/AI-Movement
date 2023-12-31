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

    private GameObject patroler;
    private NavMeshAgent patrolAgent;

    private NavMeshAgent ghostAgent;

    public Int32 initialDir;
    public Int32 wpIndex;

    public GameObject[] waypoints;
    

    void Start()
    {
        patroler = this.gameObject;
        patrolAgent = patroler.GetComponent<NavMeshAgent>();
        ghostAgent = ghost.GetComponent<NavMeshAgent>();

        initialDir = UnityEngine.Random.Range(0, 2);
        wpIndex = UnityEngine.Random.Range(0, waypoints.Length);

        ghost.transform.position = waypoints[wpIndex].transform.position;

        patrolAgent.speed = 2.5f;
        ghostAgent.speed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ghostAgent.pathPending && ghostAgent.remainingDistance < 0.5f)
        {
            wpIndex = AI.Movement.FollowPatrolPath(initialDir, wpIndex, waypoints, ghostAgent);
        }

        //AI.Movement.Seek(patrolAgent, ghost.transform);
    }
}