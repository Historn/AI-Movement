using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor.TerrainTools;

public class ThiefBehaviour : MonoBehaviour
{
    [SerializeField]
    private float wanderRadius = 8.0f;
    [SerializeField]
    private float wanderTimer = 3.5f;
    [SerializeField]
    [Range(0f, 15f)]
    public float copDetectionRadius = 6.0f;

    private GameObject thief;
    [HideInInspector]
    public GameObject cop;
    private GameObject treasure;
    private GameObject[] hidingSpots;

    private NavMeshAgent thiefAgent;

    private float coroutineTimer = 0.05f; // 20 times per second

    private Renderer rend;

    private enum ThiefState
    {
        Wander,
        Approach,
        Hide
    }

    private ThiefState currentState = ThiefState.Wander;

    void Start()
    {
        thief = this.gameObject;
        cop = GameObject.FindGameObjectWithTag("cop");
        treasure = GameObject.FindGameObjectWithTag("treasure");
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");

        thiefAgent = this.GetComponent<NavMeshAgent>();

        rend = this.GetComponent<Renderer>();

        StartCoroutine(ThiefFSM());
    }

    IEnumerator ThiefFSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case ThiefState.Wander:
                    Debug.Log("Wandering");
                    thiefAgent.speed = 1.5f;
                    Utils.SetColor(rend, Color.green);

                    currentState = isGuarded() ? ThiefState.Wander : ThiefState.Approach;

                    yield return StartCoroutine(AI.Movement.Wander(thiefAgent, wanderRadius, wanderTimer));
                    break;

                case ThiefState.Approach:
                    Debug.Log("Approaching");
                    thiefAgent.speed = 3f;
                    Utils.SetColor(rend, Color.yellow);

                    currentState = isGuarded() ? ThiefState.Wander : ThiefState.Approach;

                    if (canSteal())
                    {
                        Debug.Log("STEAL >:)");

                        if (treasure != null)
                            Destroy(treasure);

                        currentState = ThiefState.Hide;
                    }

                    AI.Movement.Seek(thiefAgent, treasure.transform);
                    break;

                case ThiefState.Hide:
                    Debug.Log("Hiding");
                    Utils.SetColor(rend, Color.red);

                    thiefAgent.speed = 4;
                        
                    while (true)
                    {
                        AI.Movement.HideCloseToCop(thiefAgent, thief, cop, hidingSpots);
                        yield return coroutineTimer;
                    }
            }

            yield return new WaitForSeconds(coroutineTimer);
        }
    }

    private bool isGuarded()
    {
        return Utils.inRange(cop, treasure, copDetectionRadius) ? true : false;
    }

    private bool canSteal()
    {
        return Utils.inRange(thief, treasure, 2) ? true : false;
    }
}