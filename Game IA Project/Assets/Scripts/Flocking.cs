using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public GameObject agentPrefab;
    public int numAgents = 20;
    public float spawnRadius = 5.0f;

    public float speed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float neighborRadius = 2.0f;
    public float separationDistance = 1.0f;

    public float randomize;


    private void Start()
    {
        // Spawn the agents
        for (int i = 0; i < numAgents; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject agent = Instantiate(agentPrefab, randomPosition, Quaternion.identity);
            agent.transform.parent = transform; // Parent the agents to the manager
        }
    }
  

    private void Update()
    {
        Vector3 move = Vector3.zero;
        Vector3 separation = Vector3.zero;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);


        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject != gameObject)
            {
                // Calculate neighbor center of mass
                Vector3 toNeighbor = neighbor.transform.position - transform.position;

                // Alignment
                move += neighbor.transform.forward;

                // Cohesion
                move += toNeighbor;

                // Separation
                if (toNeighbor.magnitude < separationDistance)
                {
                    separation -= toNeighbor.normalized / toNeighbor.magnitude;
                }
            }
        }

        move.Normalize();
        separation.Normalize();

        // Adjust the desired velocity
        move = (move + separation).normalized;

        // Rotate towards the desired velocity
        Quaternion targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
