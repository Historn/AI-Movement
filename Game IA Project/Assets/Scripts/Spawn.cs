using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject agentPrefab;
    public int numAgents = 8;
    public float spawnRadius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the agents
        for (int i = 0; i < numAgents; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject _agent = Instantiate(agentPrefab, randomPosition, Quaternion.identity);
            _agent.transform.parent = transform; // Parent the agents to the manager
        }
    }
}
