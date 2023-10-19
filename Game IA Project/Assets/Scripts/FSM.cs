using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    GameObject patroler;
    GameObject rober;

    private NavMeshAgent agent;

    private WaitForSeconds wait = new WaitForSeconds(0.05f);   // 20times/s
    delegate IEnumerator State();
    private State state;

    IEnumerator Start()
    {
        rober = this.gameObject;
        //...
        state = Wander;
        while (enabled)
            yield return StartCoroutine(state());
    }
    
    IEnumerator Wander()
    {
        Debug.Log("Wander state");

        AI_Movement.Wander(2.0f, 0.05f, agent);

        yield return;
    }

    IEnumerator Steal()
    {
        Debug.Log("Steal state");


    }

    IEnumerator Hide()
    {
        Debug.Log("Hide state");


    }
}