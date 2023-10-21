using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public static class AI
{
    // MOVEMENT ===================================================
    public static class Movement
    {
        public static void Seek(NavMeshAgent agent, Transform target)
        {
            agent.destination = target.transform.position;
        }

        public static IEnumerator Wander(NavMeshAgent agent, float wanderRadius, float wanderTimer)
        {
            Vector3 newPos = Utils.RandomNavSphere(agent.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            yield return new WaitForSeconds(wanderTimer);
        }

        public static Int32 FollowPatrolPath(Int32 initialDir, Int32 wpIndex, GameObject[] waypoints, NavMeshAgent agent)
        {
            if (initialDir == 0)
                wpIndex = (wpIndex + 1) % waypoints.Length;
            else
            {
                if (wpIndex <= 0)
                    wpIndex = waypoints.Length;

                wpIndex = (wpIndex - 1) % waypoints.Length;
            }

            agent.destination = waypoints[wpIndex].transform.position;

            return wpIndex;
        }

        public static void HideCloseToCop(NavMeshAgent thiefAgent, GameObject thief, GameObject cop, GameObject[] hidingSpots)
        {
            Func<GameObject, float> distance = (hs) => Vector3.Distance(cop.transform.position, hs.transform.position);
            GameObject hidingSpot = hidingSpots.Select(ho => (distance(ho), ho)).Min().Item2;

            Vector3 hidingDir = (hidingSpot.transform.position - cop.transform.position).normalized;
            Vector3 hidingPos = hidingSpot.transform.position + hidingDir * 2;

            // Check if the hiding spot is on the NavMesh.
            NavMeshHit hit;
            if (NavMesh.SamplePosition(hidingPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                thiefAgent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("Hiding spot is not on the NavMesh.");
            }

            Debug.DrawLine(hidingSpot.transform.position, thief.transform.position);
        }
    }


    // DETECTION ==================================================
    public static class Detection
    {
        public static bool Search(string tag, GameObject go, Camera frustum, LayerMask mask, out RaycastHit hit)
        {
            Collider[] colliders = Physics.OverlapSphere(go.transform.position, frustum.farClipPlane, mask);
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);
            Ray ray = new Ray();
            hit = default;

            foreach (Collider col in colliders)
            {
                if (col.gameObject != go && GeometryUtility.TestPlanesAABB(planes, col.bounds))
                {
                    ray.origin = go.transform.position;
                    ray.direction = (col.transform.position - go.transform.position).normalized;
                    ray.origin = ray.GetPoint(frustum.nearClipPlane);

                    if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                    {
                        if (hit.collider.gameObject.CompareTag(tag))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }


    // UTILS ======================================================
    public static class Utils
    {
        // Generate a random point within a sphere on NavMesh
        public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

            randomDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

            return navHit.position;
        }

        public static bool inRange(GameObject a, GameObject b, float range)
        {
            return Vector3.Distance(a.transform.position, b.transform.position) <= range ? true : false;
        }

        public static void DrawRay(Ray ray, Color color, float duration = 0.0f)
        {
            Debug.DrawRay(ray.origin, ray.direction * 3, color, duration);
        }
    }   
}