using UnityEngine;
using System.Collections.Generic;

public class Flock : MonoBehaviour
{
    public FlockingManager flockManager;
    private Vector3 moveDirection;
    private float speed;

    public float updateInterval = 0.2f; 
    private float timeSinceLastUpdate = 0f;

    void Update()
    {
        // Calculate the time since the last update
        timeSinceLastUpdate += Time.deltaTime;

        // Check if it's time to update behaviors
        if (timeSinceLastUpdate >= updateInterval)
        {
            // Reset the timer
            timeSinceLastUpdate = 0f;

            // Calculate the move direction for this fish based on flocking rules
            Vector3 separation = Separate() * (1 + Random.Range(-flockManager.randomFactor, flockManager.randomFactor));
            Vector3 cohesion = Cohere() * (1 + Random.Range(-flockManager.randomFactor, flockManager.randomFactor));
            Vector3 alignment = Align() * (1 + Random.Range(-flockManager.randomFactor, flockManager.randomFactor));

            // Weights for each behavior
            float separationWeight = flockManager.separationAmount;
            float cohesionWeight = flockManager.cohesionAmount;
            float alignmentWeight = flockManager.alignmentAmount;

            // Calculate the combined move direction
            moveDirection = (separation * separationWeight) + (cohesion * cohesionWeight) + (alignment * alignmentWeight);          
        }

        // Limit the speed
        speed = Mathf.Clamp(moveDirection.magnitude, 0, flockManager.maxSpeed);
        moveDirection = moveDirection.normalized * speed;

        // Apply the move direction
        transform.position += moveDirection * Time.deltaTime;

        // Rotate the fish to look in the move direction
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        ApplyBoundaries();
    }

    // Avoid collisions with nearby fish
    private Vector3 Separate()
    {
        Vector3 separationVector = Vector3.zero;
        List<GameObject> nearbyFish = flockManager.GetNearbyFish(transform.position);

        foreach (GameObject fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                Vector3 separationDir = transform.position - fish.transform.position;
                float distance = separationDir.magnitude;

                if (distance < flockManager.neighborhoodRadius)
                {
                    separationVector += separationDir.normalized / distance;
                }
            }
        }

        return separationVector;
    }

    // Move towards the center of mass of nearby fish
    private Vector3 Cohere()
    {
        Vector3 cohesionVector = Vector3.zero;
        List<GameObject> nearbyFish = flockManager.GetNearbyFish(transform.position);

        foreach (GameObject fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                cohesionVector += fish.transform.position;
            }
        }

        if (nearbyFish.Count > 0)
        {
            cohesionVector /= nearbyFish.Count;
            cohesionVector = cohesionVector - transform.position;
        }

        return cohesionVector;
    }

    // Align with the average heading of nearby fish
    private Vector3 Align()
    {
        Vector3 alignmentVector = Vector3.zero;
        List<GameObject> nearbyFish = flockManager.GetNearbyFish(transform.position);

        foreach (GameObject fish in nearbyFish)
        {
            if (fish != gameObject)
            {
                alignmentVector += fish.transform.forward;
            }
        }

        if (nearbyFish.Count > 0)
        {
            alignmentVector /= nearbyFish.Count;
        }

        return alignmentVector;
    }

    private void ApplyBoundaries()
    {
        Vector3 position = transform.position;
        Vector3 flockManagerPosition = flockManager.transform.position;
        Vector3 boundaries = flockManager.swimLimits;

        // Calculate the half-size of the fish's bounding box
        Vector3 halfSize = Vector3.one * 0.5f;

        // Calculate the minimum and maximum positions within boundaries
        Vector3 minPosition = flockManagerPosition - boundaries + halfSize;
        Vector3 maxPosition = flockManagerPosition + boundaries - halfSize;

        // Clamp the fish's position within the boundaries
        position = new Vector3(
            Mathf.Clamp(position.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(position.y, minPosition.y, maxPosition.y),
            Mathf.Clamp(position.z, minPosition.z, maxPosition.z)
        );

        // Update the fish's position
        transform.position = position;
    }
}