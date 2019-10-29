﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    
    public float maxAngle = 30.0f;          // Field of view of this human
    public float maxRadius = 15.0f;         // The farthest distance that this human can see
    //public float rotationSpeed = 5.0f;      // How fast this human rotates

    Vector3 initialPosition;                // Starting position of this human. Will return here after losing sight of raccoon
    public Transform target;                // Human target to be chased (will always be the raccoon)
    public Vector3 lastKnownLocation;       // Location where this human last saw the raccoon
    NavMeshAgent agent;                     // Pathfinding AI
    CentralHumanController CHC;             // Reference to the Central Human Controller

    public bool seesRaccoon = false;        // Flag determining whether this human can see the raccoon or not
    private bool chasing = false;           // Human status: The human knows where the raccoon is and is currently chasing her
    private bool searching = false;         // Human status: The raccoon has escaped the human's sight and the human is looking for her
    private bool idle = true;               // Human status: The human does not know where the raccoon is and is not looking for her

    // Intermediate variables
    NavMeshPath p;

    // For Animations
    public Animator anim;

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

        if (seesRaccoon)
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
        agent.stoppingDistance = 1;
        CHC = GameObject.Find("CentralHumanController").GetComponent<CentralHumanController>();
        //Debug.Log(CHC);
        p = new NavMeshPath();
        initialPosition = transform.position;

        // For Animations
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        seesRaccoon = inFOV(agent, transform, target, maxAngle, maxRadius);

        // 'lastKnownLocation' is assigned by the Central Human Controller. If a new 'lastKnownLocation' is assigned, then the raccoon has been spotted
        // somewhere and the human will head to that location if he can reach it
        if (CHC.spotted && agent.CalculatePath(lastKnownLocation, p) && onSameFloor(transform, target))
        {
            //Debug.Log("Now chasing Raccoon");
            chasing = true;
            searching = false;
            idle = false;
            agent.SetDestination(lastKnownLocation);

            // For animations
            anim.Play("scared");
        }
        //Debug.Log("seesRaccoon: " + seesRaccoon);
        //Debug.Log("Distance: " + Vector3.Distance(transform.position, lastKnownLocation));

        // The human will stop if he is at 'lastKnownLocation' and can't see the raccoon
        if (!seesRaccoon && Vector3.Distance(transform.position, lastKnownLocation) <= 2.5)
        {
            //Debug.Log("Lost Raccoon");
            chasing = false;
            searching = true;
            idle = false;
            agent.ResetPath();
        }
        
        // The human will turn to his left and right in case the raccoon is beside him
        if (searching && !chasing && !idle)
        {
            //Debug.Log("Searching for Raccoon");
            //transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f) * Time.deltaTime * rotationSpeed);
            //transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f) * Time.deltaTime * rotationSpeed);
            //transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f) * Time.deltaTime * rotationSpeed);
            searching = false;
            idle = true;
        }

        // The human will return to his original position if he can't find the raccoon
        if (idle)
        {
            //Debug.Log("Can't find Raccoon. Returning to initial position");
            agent.SetDestination(initialPosition);
        }
    }
}
