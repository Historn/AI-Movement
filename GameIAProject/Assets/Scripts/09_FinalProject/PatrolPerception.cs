using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class PatrolPerception : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public bool isDetected = false;

    public Camera frustum;
    public LayerMask mask;

    private RaycastHit hit;

    private GameObject patroler;
    private NavMeshAgent patrolAgent;

    public Int32 initialDir;
    public Int32 wpIndex;

    public GameObject[] waypoints;

    void Start()
    {

        patroler = this.gameObject;

        patrolAgent = patroler.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Thief").transform;
        frustum = patroler.GetComponentInChildren<Camera>();

        patroler.transform.position = waypoints[wpIndex].transform.position;

        initialDir = UnityEngine.Random.Range(0, 2);
        wpIndex = UnityEngine.Random.Range(0, waypoints.Length);
    }

    private void Update()
    {
        if (!isDetected)
        {
            if (AI.Detection.Search("Thief", this.gameObject, frustum, mask, out hit))
                this.transform.parent.BroadcastMessage("Detected", hit.collider.gameObject.transform);

            if (!patrolAgent.pathPending && patrolAgent.remainingDistance < 0.5f)
            {
                wpIndex = AI.Movement.FollowPatrolPath(initialDir, wpIndex, waypoints, patrolAgent);
            }
        }
        else
        {
            AI.Movement.Seek(patrolAgent, target);
        }
    }

    void Detected(Transform _target)
    {
        target = _target;
        isDetected = true;
    }
}
