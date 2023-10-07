using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flock : MonoBehaviour
{
    public FlockingManager flockingManager;

    private float speed;
    private Vector3 moveDirection;
    public Vector3 baseRotation;
    public Vector3 acceleration;
    public Vector3 velocity;

    private Vector3 Position
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            gameObject.transform.position = value;
        }
    }

    private void Start()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), Mathf.Cos(angle));
    }

    private void Update()
    {
        var boidColliders = Physics.OverlapSphere(Position, flockingManager.neighborhoodRadius);
        var boids = boidColliders.Select(o => o.GetComponent<Flock>()).ToList();
        boids.Remove(this);

        Flocking(boids);
        UpdateVelocity();
        UpdatePosition();
        UpdateRotation();

        Bounds();
    }

    private void Flocking(IEnumerable<Flock> boids)
    {
        var alignment = Alignment(boids);
        var separation = Separation(boids);
        var cohesion = Cohesion(boids);

        acceleration = flockingManager.alignmentAmount * alignment + flockingManager.cohesionAmount * cohesion + flockingManager.separationAmount * separation;
    }

    public void UpdateVelocity()
    {
        velocity += acceleration;
        velocity = LimitMagnitude(velocity, flockingManager.maxSpeed);
    }

    private void UpdatePosition()
    {
        Position += velocity * Time.deltaTime;
    }

    private void UpdateRotation()
    {
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
    }

    private Vector3 Alignment(IEnumerable<Flock> boids)
    {
        var velocitySum = Vector3.zero;
        if (!boids.Any()) return velocitySum;

        foreach (var boid in boids)
        {
            velocitySum += boid.velocity;
        }
        velocitySum /= boids.Count();

        var steer = Steer(velocitySum.normalized * flockingManager.maxSpeed);
        return steer;
    }

    private Vector3 Cohesion(IEnumerable<Flock> boids)
    {
        if (!boids.Any()) return Vector3.zero;

        var sumPositions = Vector3.zero;
        foreach (var boid in boids)
        {
            sumPositions += boid.Position;
        }
        var average = sumPositions / boids.Count();
        var direction = average - Position;

        var steer = Steer(direction.normalized * flockingManager.maxSpeed);
        return steer;
    }

    private Vector3 Separation(IEnumerable<Flock> boids)
    {
        var direction = Vector3.zero;
        boids = boids.Where(o => DistanceTo(o) <= flockingManager.neighborhoodRadius / 2);
        if (!boids.Any()) return direction;

        foreach (var boid in boids)
        {
            var difference = Position - boid.Position;
            direction += difference.normalized / difference.magnitude;
        }
        direction /= boids.Count();

        var steer = Steer(direction.normalized * flockingManager.maxSpeed);
        return steer;
    }

    private Vector3 Steer(Vector3 desired)
    {
        var steer = desired - velocity;
        steer = LimitMagnitude(steer, flockingManager.maxForce);

        return steer;
    }

    private float DistanceTo(Flock boid)
    {
        return Vector3.Distance(boid.transform.position, Position);
    }

    private Vector3 LimitMagnitude(Vector3 baseVector, float maxMagnitude)
    {
        if (baseVector.sqrMagnitude > maxMagnitude * maxMagnitude)
        {
            baseVector = baseVector.normalized * maxMagnitude;
        }
        return baseVector;
    }

    void Bounds()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Keep the fish inside bounds
        Vector3 newPosition = transform.position;

        if (newPosition.x < -flockingManager.swimLimits.x || newPosition.x > flockingManager.swimLimits.x ||
            newPosition.y < -flockingManager.swimLimits.y || newPosition.y > flockingManager.swimLimits.y ||
            newPosition.z < -flockingManager.swimLimits.z || newPosition.z > flockingManager.swimLimits.z)
        {
            moveDirection = -moveDirection; // Reverse direction
        }
    }
}