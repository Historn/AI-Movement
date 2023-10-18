using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private Transform target;       
    private NavMeshAgent agent;     

    private float timer;            

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // Start the wandering behavior
        StartCoroutine(AI_Movement.Wander(wanderRadius, wanderTimer, agent));
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to choose a new wander destination
        if (timer >= wanderTimer)
        {
            StartCoroutine(AI_Movement.Wander(wanderRadius, wanderTimer, agent));
            timer = 0;
        }
    }
}