using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float neighborRadius = 2.0f;
    public float separationDistance = 1.0f;

    public int numSeagulls;
    public GameObject seagull;

    public float randomize;


    private void Start()
    {
        for (int i = 0; i < numSeagulls; i++)
        {
            //seagulls = new GameObject[numSeagulls];

            Vector3 pos = this.transform.position;
            Vector3 randomize = Random.insideUnitSphere;
            seagull = (GameObject)Instantiate(seagull, pos, Quaternion.LookRotation(randomize));
            //seagull.GetComponent<Flock>().myManager = this;
        }
    }
  

    private void Update()
    {
        Vector3 move = Vector3.zero;
        Vector3 separation = Vector3.zero;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);

        // Calculate neighbor center of mass
        foreach (var neighbor in neighbors)
        {
            Vector3 centerOfMass;

            if (neighbor.gameObject.CompareTag("gaviota"))
            {
                //center of mass
            }
        }

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
