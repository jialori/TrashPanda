using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    public Transform target;                // Human target to be chased (will always be the raccoon)
    public Vector3 lastKnownLocation;       // Location where this human last saw the raccoon (set to the human's original location by default)
    public float maxAngle = 30.0f;          // Field of view of this human
    public float maxRadius = 15.0f;         // The farthest distance that this human can see
    public bool isInFOV = false;            // Flag to determine whether this human can see the raccoon or not
    NavMeshAgent agent;                     // Pathfinding AI

    /*
    // Outline detection cones in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFOV)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    // Determine if the player has been seen by this human
    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        // Retrieve all objects within this human's field of view
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        // For each object retrieved
        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                // If the object being examined is the player
                if (overlaps[i].transform == target)
                {
                    
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    // If the player is within the field of view angles
                    if (angle <= maxAngle)
                    {
                        
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        // If the player is close enough to the human to be seen
                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            // If the human can directly see the player (i.e. line of sight is not blocked by wall or bush)
                            if (hit.transform == target)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
    */

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //lastKnownLocation = transform.position;
    }

    void Update() 
    {
        //isInFOV = inFOV(transform, target, maxAngle, maxRadius);

        // The human will chase the raccoon while he can see her. If the raccoon leaves the human's sight, the human will head to the last place where the human saw her
        /*
        if (isInFOV)
        {
            lastKnownLocation = target.position;
        }
        */
        if (agent.hasPath)
        {
            agent.SetDestination(target.position);
        }
        
    }
}
