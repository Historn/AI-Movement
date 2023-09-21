using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public float maxJump;

    // Update is called once per frame
    void Update()
    {
        Seek();
    }

    void Seek()
    {
        agent.destination = target.transform.position;
        if ()
        {

        }
    }
}
