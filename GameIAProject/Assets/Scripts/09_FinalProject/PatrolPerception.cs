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

    public GameObject parentWaypoint;
    public List<GameObject> waypoints = new List<GameObject>();

    private bool isPaused = false;

    void Start()
    {
        patroler = this.gameObject;

        patrolAgent = patroler.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Thief").transform;
        frustum = patroler.GetComponentInChildren<Camera>();

        // Check if a parent object is assigned
        if (parentWaypoint != null)
        {
            // Iterate through the children and add them to the list
            foreach (Transform child in parentWaypoint.transform)
            {
                waypoints.Add(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("Parent object not assigned!");
        }

        patroler.transform.position = waypoints[wpIndex].transform.position;

        initialDir = 0;
        wpIndex = 0;

        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (!isDetected)
            {
                if (AI.Detection.Search("Thief", this.gameObject, frustum, mask, out hit))
                    this.transform.parent.BroadcastMessage("Detected", hit.collider.gameObject.transform);

                if (!isPaused)
                {
                    if (waypoints[wpIndex].tag.Equals("stop") && !patrolAgent.pathPending && patrolAgent.remainingDistance < 0.5f)
                    {
                        isPaused = true;
                    }
                    else if (!patrolAgent.pathPending && patrolAgent.remainingDistance < 0.4f)
                    {
                        wpIndex = AI.Movement.FollowPatrolPath(initialDir, wpIndex, waypoints.ToArray(), patrolAgent);
                    }
                }
            }
            else
            {
                // Handle detection or seek logic when the patroller is detected
                AI.Movement.Seek(patrolAgent, target);
            }

            if (isPaused)
            {
                patrolAgent.isStopped = true;
            }

            yield return new WaitForSeconds(isPaused ? 3 : 0);

            if (isPaused)
            {
                isPaused = false;
                patrolAgent.isStopped = false;
                wpIndex = AI.Movement.FollowPatrolPath(initialDir, wpIndex, waypoints.ToArray(), patrolAgent);
            }          
        }
    }

    void Detected(Transform _target)
    {
        target = _target;
        isDetected = true;
    }
}