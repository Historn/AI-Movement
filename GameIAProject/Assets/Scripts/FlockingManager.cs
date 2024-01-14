using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockingManager : MonoBehaviour
{
    [Header("Spawn")]
    public Flock fishPrefab;
    public int numEntities = 20;
    [Range(1, 10)]
    public float bounds = 5f;

    [Header("Type")]
    public bool is3D = true;

    [Header("Speed")]
    [Range(0, 10)]
    public float minSpeed = 1f;

    [Range(0, 10)]
    public float maxSpeed = 3f;
    

    [Header("Detection Distance")]
    [Range(0, 10)]
    public float cohesionDistance = 5f;

    [Range(0, 10)]
    public float separationDistance = 2f;

    [Range(0, 10)]
    public float alignmentDistance = 2f;


    [Header("Weights")]
    [Range(0, 5)]
    public float randomFactor = 0.2f;

    [Range(0, 10)]
    public float cohesionWeight = 2f;

    [Range(0, 10)]
    public float separationWeight = 1f;

    [Range(0, 10)]
    public float alignmentWeight = 0.2f;

    [Range(0, 20)]
    public float boundsWeight = 10f;


    public Flock[] allFish { get; set; }


    void Start()
    {
        allFish = new Flock[numEntities];
        float posY = this.transform.position.y;

        for (int i = 0; i < numEntities; i++)
        {
            Vector3 spawnPos = this.transform.position +
                new Vector3(
                    Random.Range(1, 5),
                    is3D ? Random.Range(1, 5) : posY,
                    Random.Range(1, 5));


            Quaternion spawnRot = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

            allFish[i] = Instantiate(fishPrefab, spawnPos, spawnRot);
            allFish[i].AssignFlockManager(this);
            allFish[i].SetSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }

    }

    public List<Flock> GetNearbyFish(Vector3 position, float detectionDistance)
    {
        List<Flock> nearbyFish = new List<Flock>();

        foreach (Flock fish in allFish)
        {
            if (fish != null && fish != gameObject)
            {
                float distance = Vector3.Distance(position, fish.transform.position);

                if (distance <= detectionDistance)
                {
                    nearbyFish.Add(fish);
                }
            }
        }

        return nearbyFish;
    }
}