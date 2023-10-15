using UnityEngine;
using System.Collections.Generic;

public class Flock : MonoBehaviour
{
    private FlockingManager flockManager;
    private Vector3 moveDirection;
    private float speed;
    private Vector3 currentVelocity;


    public Transform myTransform { get; set; }

    public void AssignFlockManager(FlockingManager flockingManager)
    {
        flockManager = flockingManager;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        Vector3 cohesion = CohereVector() * flockManager.cohesionWeight;
        Vector3 separation = SeparateVector() * flockManager.separationWeight;
        Vector3 alignment = AlignVector() * flockManager.alignmentWeight;
        Vector3 bounds = BoundsVector();

        //(1 + Random.Range(-flockManager.randomFactor, flockManager.randomFactor));

        moveDirection = cohesion + separation + alignment + bounds;

        moveDirection = Vector3.SmoothDamp(myTransform.forward, moveDirection, ref currentVelocity, 6.5f);

        //speed = Mathf.Clamp(moveDirection.magnitude, flockManager.minSpeed, flockManager.maxSpeed);
        moveDirection = moveDirection.normalized * speed;

        if (moveDirection == Vector3.zero)
            moveDirection = myTransform.forward;

        myTransform.forward = moveDirection;
        myTransform.position += moveDirection * Time.deltaTime;
    }

    // Move towards the center of mass of nearby fish
    private Vector3 CohereVector()
    {
        Vector3 cohesionVector = Vector3.zero;
        List<Flock> nearbyFish = flockManager.GetNearbyFish(myTransform.position, flockManager.cohesionDistance);

        foreach (Flock fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                cohesionVector += fish.myTransform.position;
            }
        }

        if (nearbyFish.Count > 0)
        {
            cohesionVector /= nearbyFish.Count;
            cohesionVector = cohesionVector - myTransform.position;
        }

        return cohesionVector.normalized;
    }

    // Avoid collisions with nearby fish
    private Vector3 SeparateVector()
    {
        Vector3 separationVector = Vector3.zero;
        List<Flock> nearbyFish = flockManager.GetNearbyFish(myTransform.position, flockManager.separationDistance);

        foreach (Flock fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                Vector3 separationDir = myTransform.position - fish.myTransform.position;
                float distance = separationDir.magnitude;

                if (distance < flockManager.separationDistance)
                {
                    separationVector += separationDir.normalized / distance;
                }
            }
        }

        return separationVector.normalized;
    }

    // Align with the average heading of nearby fish
    private Vector3 AlignVector()
    {
        Vector3 alignmentVector = Vector3.zero;
        List<Flock> nearbyFish = flockManager.GetNearbyFish(myTransform.position, flockManager.alignmentDistance);

        foreach (Flock fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                alignmentVector += fish.myTransform.forward;
            }
        }

        if (nearbyFish.Count > 0)
        {
            alignmentVector /= nearbyFish.Count;
        }

        return alignmentVector.normalized;
    }

    private Vector3 BoundsVector()
    {
        var offsetToCenter = flockManager.transform.position - myTransform.position;
        bool isNearCenter = (offsetToCenter.magnitude >= flockManager.bounds * 0.9f);
        return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
    }
}