using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spawn
{
    public static void RandomSpawn(GameObject spawner, GameObject agentPrefab, int numAgents, float spawnRadius) 
    {
        // Spawn the agents
        for (int i = 0; i < numAgents; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject _agent = GameObject.Instantiate(agentPrefab, randomPosition, Quaternion.identity);
            _agent.transform.parent = spawner.transform; // Parent the agents to the manager
        }
    }

    public static void SlotsSpawn(int melee, GameObject meleePrefab, 
                                int missile, GameObject missilePrefab, 
                                int magic, GameObject magicPrefab, GameObject ghost) 
    {
        int front = 2 * melee / 3;
        int rear = melee - front;
        createRow(front, -2f, meleePrefab, ghost);
        createRow(missile, -4f, missilePrefab, ghost);
        createRow(magic, -6f, magicPrefab, ghost);
        createRow(rear, -8f, meleePrefab, ghost);
    }

    static void createRow(int num, float z, GameObject pf, GameObject ghost)
    {
        float pos = 1 - num;
        for (int i = 0; i < num; ++i)
        {
            Vector3 position = ghost.transform.TransformPoint(new Vector3(pos, 0f, z));
            GameObject temp = GameObject.Instantiate(pf, position, ghost.transform.rotation);
            temp.AddComponent<Formation>();
            temp.GetComponent<Formation>().pos = new Vector3(pos, 0, z);
            temp.GetComponent<Formation>().target = ghost;
            pos += 2f;
        }
    }
}

