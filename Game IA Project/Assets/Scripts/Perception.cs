using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Perception : MonoBehaviour
{
    [SerializeField]
    private float wanderRadius = 10f;
    [SerializeField]
    private float wanderTimer = 5f;

    public Transform target;
    private NavMeshAgent agent;

    public bool isDetected = false;

    public Camera frustum;
    public LayerMask mask;

    private RaycastHit hit;

    Renderer rend;

    IEnumerator Start()
    {
        rend = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        Utils.SetColor(rend, Color.yellow);

        while (!isDetected)
            yield return StartCoroutine(AI.Movement.Wander(agent, wanderRadius, wanderTimer));
    }

    private void Update()
    {
        if (!isDetected)
        {
            if (AI.Detection.Search("Character", this.gameObject, frustum, mask, out hit))
                this.transform.parent.BroadcastMessage("Detected", hit.collider.gameObject.transform);
        }
        else
        {
            AI.Movement.Seek(agent, target);
        }
    }

    void Detected(Transform _target)
    {
        target = _target;
        isDetected = true;
        Utils.SetColor(rend, Color.red);
    }
}