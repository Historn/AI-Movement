using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Perception : MonoBehaviour
{
    [SerializeField]
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    public Transform target;
    private NavMeshAgent agent;

    public GameObject agentPrefab;
    public int numAgents = 8;
    public float spawnRadius = 2.0f;
    public float neighborRadius = 2.0f;

    public bool isDetected = false;

    public Camera frustum;
    public LayerMask mask;

    private RaycastHit hit;

    Renderer rend;

    IEnumerator Start()
    {
        rend = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        SetColor();

        while (enabled)
            yield return WanderSeek();
    }

    IEnumerator WanderSeek()
    {
        while (!isDetected)
        {
            if (AI.Detection.Search("Character", this.gameObject, frustum, mask, out hit))
                this.transform.parent.BroadcastMessage("Detected", hit.collider.gameObject.transform);

            yield return AI.Movement.Wander(agent, wanderRadius, wanderTimer);
        }

        AI.Movement.Seek(agent, target);
    }

    void Detected(Transform _target)
    {
        target = _target;
        isDetected = true;
        SetColor();
    }

    void SetColor()
    {
        if (isDetected)
            rend.material.SetColor("_Color", Color.red);
        else
            rend.material.SetColor("_Color", Color.yellow);
    }
}