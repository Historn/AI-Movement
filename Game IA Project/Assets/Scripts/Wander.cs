using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;

    float heading;
    Vector3 targetRotation;

    void Awake()
    {
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);

        transform.position += forward * Time.deltaTime;

        agent.transform.position = this.transform.position;
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingCalculation();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void NewHeadingCalculation()
    {
        var floor = transform.eulerAngles.y - maxHeadingChange;
        var ceil = transform.eulerAngles.y + maxHeadingChange;
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }
}