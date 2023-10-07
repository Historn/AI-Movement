using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);

    //[Header("Fish Settings")]
    //[Range(0.0f, 5.0f)]
    //public float minSpeed = 1;
    //[Range(0.0f, 5.0f)]
    //public float maxSpeed = 3;
    //[Range(1.0f, 10.0f)]
    //public float neighbourDistance = 2;
    //[Range(0.0f, 5.0f)]
    //public float rotationSpeed = 2;

    //[Header("Flock Settings")]
    //[Range(0.0f, 5.0f)]
    //public float alignmentWeight = 1.0f;
    //[Range(0.0f, 5.0f)]
    //public float cohesionWeight = 1.0f;
    //[Range(0.0f, 5.0f)]
    //public float separationWeight = 1.0f;

    [Header("Flock Settings")]
    [Range(0, 10)]
    public float maxSpeed = 1f;

    [Range(.1f, .5f)]
    public float maxForce = .03f;

    [Range(1, 10)]
    public float neighborhoodRadius = 3f;

    [Range(0, 3)]
    public float separationAmount = 1f;

    [Range(0, 3)]
    public float cohesionAmount = 1f;

    [Range(0, 3)]
    public float alignmentAmount = 1f;


    // Use this for initialization
    void Start()
    {
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);

            Flock flockComponent = allFish[i].GetComponent<Flock>();
            if (flockComponent != null)
            {
                flockComponent.flockingManager = this;
            }
        }

    }
}