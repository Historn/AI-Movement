using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Perception : MonoBehaviour
{
    Renderer rend;

    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    public Transform target;
    private NavMeshAgent agent;

    private float timer;

    public GameObject agentPrefab;
    public int numAgents = 8;
    public float spawnRadius = 2.0f;
    public float neighborRadius = 2.0f;

    public bool isDetected = false;

    public Camera frustum;
    public LayerMask mask;

    

    void Start()
    {
        rend = GetComponent<Renderer>();

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // Start the wandering behavior
        StartCoroutine(Wander());       
    }

    void Update()
    {
        timer += Time.deltaTime;

        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);

                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                {
                    if (hit.collider.gameObject.CompareTag("Character"))
                    {
                        this.transform.parent.BroadcastMessage("Detected", hit.collider.gameObject.transform);
                    }
                }
    

            }
        }

        if (isDetected)
        {
            Seek();
        }
        else
        {
            // Check if it's time to choose a new wander destination
            if (timer >= wanderTimer)
            {
                StartCoroutine(Wander());
                timer = 0;
            }
        }

        SetColor();
    }

    IEnumerator Wander()
    {
        // Pick a random point within the wanderRadius
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

        // Set the NavMeshAgent's destination to the new position
        agent.SetDestination(newPos);

        // Wait for a short time before choosing a new destination
        yield return new WaitForSeconds(wanderTimer);
    }

    // Generate a random point within a sphere (for wandering)
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void Seek()
    {
        agent.destination = target.transform.position;
    }

    void Detected(Transform _target)
    {
        target = _target;
        isDetected = true;
    }

    void SetColor()
    {
        if (isDetected)
            rend.material.SetColor("_Color", Color.red);
        else
            rend.material.SetColor("_Color", Color.yellow);
    }

}
