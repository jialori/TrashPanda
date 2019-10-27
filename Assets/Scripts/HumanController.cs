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

    // Intermediate variables
    NavMeshPath p;

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

        if (isInFOV)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * maxRadius);
        }        

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    // Determine if the human and the player are on the same floor
    public static bool onSameFloor(Transform checkingObject, Transform target)
    {
        return target.position.y - 2 <= checkingObject.position.y && checkingObject.position.y <= target.position.y + 2;
    }

    // Determine if the player has been seen by this human
    public static bool inFOV(NavMeshAgent nav, Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        /*
        // Retrieve all objects within this human's field of view
        Collider[] overlaps = new Collider[30];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        
        // For each object retrieved
        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                
                // If the object being examined is the player
                if (overlaps[i].transform == target)
                {
                    
                }
            }
        }
        */
        Vector3 directionBetween = (target.position - checkingObject.position).normalized;
        directionBetween.y *= 0;

        float angle = Vector3.Angle(checkingObject.forward, directionBetween);

        // If the player is within the field of view angles
        if (angle <= maxAngle)
        {
            //Debug.Log(1);
            //Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);

            // If the player is close enough to the human to be seen
            if (Vector3.Distance(checkingObject.position, target.position) < maxRadius)
            {
                //Debug.Log(2);
                NavMeshHit hit;

                // If the player and human are on the same floor
                if (onSameFloor(checkingObject, target))
                {
                    //Debug.Log(3);
                    // If the human can directly see the player (i.e. line of sight is not blocked by wall or bush)
                    if (!nav.Raycast(target.position, out hit))
                    {

                        return true;
                    }
                }
                
            }
        }

        return false;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        p = new NavMeshPath();
        //lastKnownLocation = transform.position;
    }

    void Update() 
    {
        isInFOV = inFOV(agent, transform, target, maxAngle, maxRadius);

        // The human will chase the raccoon while he can see her and can reach her. If the raccoon leaves the human's sight, the human will head to the last place where the human saw her
        /*
        if (isInFOV)
        {
            lastKnownLocation = target.position;
        }
        */

        if (agent.CalculatePath(lastKnownLocation, p) && onSameFloor(transform, target))
        {
            //Debug.Log("hasPath");
            agent.SetDestination(lastKnownLocation);
        }
        
    }
}
