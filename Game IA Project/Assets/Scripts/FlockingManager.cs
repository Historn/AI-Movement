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


    [Header("Flock Settings")]

    [Range(0, 5)]
    public float randomFactor = 0.2f;

    [Range(0, 10)]
    public float maxSpeed = 1f;

    [Range(1, 10)]
    public float neighborhoodRadius = 1f;

    [Range(0, 3)]
    public float separationAmount = 1f;

    [Range(0, 3)]
    public float cohesionAmount = 1f;

    [Range(0, 3)]
    public float alignmentAmount = 1f;


    void Start()
    {
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
           

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

            Flock flockComponent = allFish[i].GetComponent<Flock>();
            if (flockComponent != null)
            {
                flockComponent.flockManager = this;
            }
        }

    }

    public List<GameObject> GetNearbyFish(Vector3 position)
    {
        List<GameObject> nearbyFish = new List<GameObject>();

        foreach (GameObject fish in allFish)
        {
            if (fish != null && fish != gameObject)
            {
                float distance = Vector3.Distance(position, fish.transform.position);

                if (distance <= neighborhoodRadius)
                {
                    nearbyFish.Add(fish);
                }
            }
        }

        return nearbyFish;
    }
}