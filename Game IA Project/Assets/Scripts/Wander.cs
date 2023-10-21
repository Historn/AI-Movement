using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 4f;
    
    private NavMeshAgent agent;

    private float timer;            

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        while (enabled)
            yield return StartCoroutine(AI.Movement.Wander(agent, wanderRadius, wanderTimer));
    }
}