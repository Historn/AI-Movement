using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Perception : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    public Transform target;
    private NavMeshAgent agent;

    private float timer;

    public GameObject agentPrefab;
    public int numAgents = 8;
    public float spawnRadius = 2.0f;

    [SerializeField]
    bool isDetected = false;

    public Camera frustum;
    public LayerMask mask;

    void Awake()
    {
        // Spawn the agents
        for (int i = 0; i < numAgents; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject _agent = Instantiate(agentPrefab, randomPosition, Quaternion.identity);
            _agent.transform.parent = transform; // Parent the agents to the manager
        }
    }

    void Start()
    {
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
                    if (hit.collider.gameObject.CompareTag("Character"))
                    {
                        BroadcastMessage("Detected", hit.collider.gameObject.transform);
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
        Vector3 randomDirection = Random.insideUnitSphere * distance;

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

}