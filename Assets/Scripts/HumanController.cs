using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxAngle = 30.0f;            // Field of view of this worker
    [SerializeField] private float maxRadius = 15.0f;           // The farthest distance that this worker can see
    [SerializeField] private float hearingRadius = 40.0f;       // The farthest distance that this worker can hear
    [SerializeField] private int level;                         // The floor this worker is on 
    Vector3 initialPosition;                                    // Starting position of this worker. Will return here after losing sight of raccoon
    Quaternion initialDirection;                                // Direction this worker initially faces. Will rotate to face this direction after returning to initialPosition
    NavMeshAgent agent;                                         // Pathfinding AI
    CentralHumanController CHC;                                 // Reference to the Central Human Controller
    List<Breakable> breakableObjects;                           // List of breakable objects on this worker's floor
    List<Knockable> knockableObjects;                           // List of knockable objects on this worker's floor
    
    
    [Header("Target")]
    [SerializeField] private Transform target;              // Worker target to be chased (will always be the raccoon)
    public Vector3 lastKnownLocation;                       // Location where this worker last saw the raccoon
    private float rotationSpeed = 5.0f;                     // How fast this worker rotates
    private float attackCooldown = 10.0f;                   // The cooldown timer for the worker's stun attack
    public bool seesRaccoon = false;                        // Flag determining whether this worker can see the raccoon or not
    private bool canAttack = true;                          // Flag determining whether this worker can attack
    private bool chasing = false;                           // Worker status: The worker knows where the raccoon is and is currently chasing her
    private bool searching = false;                         // Worker status: The raccoon has escaped the worker's sight and the worker is looking for her
    private bool investigating = false;                     // Worker status: A noise has caught this worker's attention and the worker is investigating
    private bool idle = true;                               // Worker status: The worker does not know where the raccoon is and is not looking for her

    [Header("Voice Over")]
    AudioSource WorkerAudio;                // Audiosource files and script
    public AudioClip[] workerChaseVO;       // Chasing VO
    public AudioClip[] workerStunVO;        // Chasing VO
    public float replayInterval;            // Time till replay is ready
    private float _timer = 0;
    private bool alreadyPlayed = false;     // Helps with OneShot trigger to only have one instance of sound    

    // Animation
    private Animator anim;

    // For Play/Pause toggle 
    private bool pause = false;

    // Intermediate variables
    Breakable[] B;
    Knockable[] K;
    NavMeshPath p;
    [SerializeField] private string id;                     // This worker's identifier. Used for debugging
    List<Breakable> destroyedObjects;                       // List of destroyed objects. Used to remove objects from 'breakableObjects'
    List<Knockable> toppledObjects;                         // List of knocked over objects. Used to remove objects from 'knockableObjects'

    private void Awake()
    {
        workerChaseVO = Resources.LoadAll<AudioClip>("Audio/ChaseVO");
        workerStunVO = Resources.LoadAll<AudioClip>("Audio/StunVO");
        Debug.Log("[HumanController] workerChaseVO.length: " + workerChaseVO.Length);
        Debug.Log("[HumanController] workerStunVO.length: " + workerStunVO.Length);

    }

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

    // Determine if this worker and the object 'target' are on the same floor
    public bool onSameFloor(Transform target)
    {
        if (target.TryGetComponent(out RaccoonController raccoon))
        {
            return level == raccoon.level;
        }
        else if (target.TryGetComponent(out Breakable breakable))
        {
            return level == breakable.level;
        }
        else if (target.TryGetComponent(out Knockable knockable))
        {
            return level == knockable.level;
        }
        else
        {
            return false;
        }
    }

    // Determine if the player has been seen by this worker
    public bool inFOV(NavMeshAgent nav, Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Vector3 directionBetween = (target.position - checkingObject.position).normalized;
        directionBetween.y *= 0;

        float angle = Vector3.Angle(checkingObject.forward, directionBetween);

        // If the player is within the field of view angles
        if (angle <= maxAngle)
        {
            // If the player is close enough to the worker to be seen
            if (Vector3.Distance(checkingObject.position, target.position) < maxRadius)
            {
                // If the player and worker are on the same floor
                if (onSameFloor(target))
                {
                    // If the worker can directly see the player (i.e. line of sight is not blocked by wall or bush)
                    if (!nav.Raycast(target.position, out _))
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
        // Register itself at GameManager
        GameManager.instance.Workers.Add(this);

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2f;
        CHC = GameObject.Find("CentralHumanController").GetComponent<CentralHumanController>();
        //Debug.Log(CHC);
        p = new NavMeshPath();
        initialPosition = transform.position;
        initialDirection = transform.rotation;

        // Retrieve all breakable objects on the same floor as this worker
        breakableObjects = new List<Breakable>();
        destroyedObjects = new List<Breakable>();
        B = FindObjectsOfType<Breakable>();
        
        for (int i = 0; i < B.Length; i++)
        {
            //Debug.Log(B[i].ToString() + " level: " + B[i].level.ToString());
            if (onSameFloor(B[i].transform))
            {
                breakableObjects.Add(B[i]);
                //Debug.Log(B[i].ToString() + " was added");
            }
            
        }
        /*
        Debug.Log(breakableObjects.Count);
        for (int i = 0; i < breakableObjects.Count; i++)
        {
            Debug.Log(breakableObjects[i]);
        }
        */

        // Retrieve all knockable objects on the same floor as this worker
        knockableObjects = new List<Knockable>();
        toppledObjects = new List<Knockable>();
        K = FindObjectsOfType<Knockable>();

        for (int i = 0; i < K.Length; i++)
        {
            //Debug.Log(K[i].ToString() + " level: " + K[i].level.ToString());
            if (onSameFloor(K[i].transform))
            {
                knockableObjects.Add(K[i]);
                //Debug.Log(K[i].ToString() + " was added");
            }
        }
        /*
        Debug.Log(knockableObjects.Count);
        for (int i = 0; i < knockableObjects.Count; i++)
        {
            Debug.Log(knockableObjects[i]);
        }
        */

        anim = gameObject.GetComponent<Animator>();
        //Audio Component
        WorkerAudio = GetComponent<AudioSource>();
    }

    void Update() 
    {
        if (pause) return;

        /*
        if (!onSameFloor(target))
            return;
        */

        _timer += Time.deltaTime;
        if (alreadyPlayed && _timer > replayInterval)
        {
            alreadyPlayed = false;
        }

        seesRaccoon = inFOV(agent, transform, target, maxAngle, maxRadius);
        /*
        if (seesRaccoon)
        {
            Debug.Log("detected raccoon");
        }
        */
        // Animation
        anim.SetBool("scared", seesRaccoon);
        

        // 'lastKnownLocation' is assigned by the Central Human Controller. If a new 'lastKnownLocation' is assigned, then the raccoon has been spotted
        // somewhere and the worker will head to that location if he can reach it

        // Debug Animation
        //if (GameObject.Find("C_worker_Rigged").transform == transform)
        //{
        //Debug.Log(agent.CalculatePath(lastKnownLocation, p));
        //Debug.Log(p.status);
        //}

        // When the worker is at the same floor and not reach lastKnownLocation yet
        if (CHC.spotted && agent.CalculatePath(lastKnownLocation, p) 
            && System.Math.Abs(transform.position.x - lastKnownLocation.x) > 2 &&
            System.Math.Abs(transform.position.z - lastKnownLocation.z) > 2 &&
            onSameFloor(target))
        {
            //Debug.Log("Now chasing Raccoon");
            chasing = true;
            searching = false;
            idle = false;
            investigating = false;
            agent.ResetPath();
            agent.SetDestination(lastKnownLocation);
            //Debug.Log(id + " - worker's level: " + level.ToString() + ", raccoon's level: " + target.GetComponent<RaccoonController>().level.ToString());

            //Audio trigger for sighting Raccoon
            if (!seesRaccoon && !alreadyPlayed)
            {
                chaseVO();
            }

            // Animation
            anim.SetBool("chasing", true);
            anim.SetBool("scared", false);
        }
        //Debug.Log("seesRaccoon: " + seesRaccoon);
        //Debug.Log("Distance: " + Vector3.Distance(transform.position, lastKnownLocation));

        /*
        // If the raccoon leaves the nav mesh, the worker will return to the 'idle' state
        if (!agent.pathPending)
        {
            Debug.Log("Raccoon not in nav mesh");
            chasing = false;
            searching = false;
            idle = true;
        }
        */

        // Raccoon is in range
        if (seesRaccoon && System.Math.Abs(transform.position.x - lastKnownLocation.x) <= 2 &&
            System.Math.Abs(transform.position.z - lastKnownLocation.z) <= 2 && canAttack)
        {
            // Stun attack here
            //Debug.Log("stun attack used");
            target.GetComponent<RaccoonController>().isStunned = true;
            anim.SetBool("attack", true);

            // Start cooldown after attack
            canAttack = false;
            attackCooldown = 10.0f;

            //Audio
            if (!canAttack)
            {
                stunVO();
            }

            // Animation
            //anim.Play("kicking");
        }
        else if (!canAttack)
        {
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }
            else
            {
                canAttack = true;
                //Debug.Log("stun attack ready");
            }
            anim.SetBool("attack", false);
        }

        // The worker will stop if he is at 'lastKnownLocation' and can't see the raccoon
        if (!seesRaccoon && System.Math.Abs(transform.position.x - lastKnownLocation.x) <= 3 && 
            System.Math.Abs(transform.position.z - lastKnownLocation.z) <= 3)
        {
            //Debug.Log("Lost Raccoon");
            chasing = false;
            searching = true;
            investigating = false;
            idle = false;
            agent.ResetPath();

            // Animation
            anim.SetBool("chasing", false);
        }
        /*
        if (System.Math.Abs(transform.position.x - lastKnownLocation.x) > 2 || System.Math.Abs(transform.position.z - lastKnownLocation.z) > 2)
        {
            Debug.Log("position: " + transform.position.ToString() + ", lastKnownLocation: " + lastKnownLocation.ToString());
        }
        */

        // The worker will turn to his left and right in case the raccoon is beside him
        if (searching && !chasing && !idle)
        {
            /*
            Debug.Log("Searching for Raccoon");
            agent.isStopped = true;
            transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f), Time.deltaTime * rotationSpeed);
            transform.Rotate(new Vector3(0.0f, 270.0f, 0.0f), Time.deltaTime * rotationSpeed);
            transform.Rotate(new Vector3(0.0f, 270.0f, 0.0f), Time.deltaTime * rotationSpeed);
            agent.isStopped = false;
            */
            searching = false;
            idle = true;
        }
        
        // For each breakable object on this worker's floor
        for (int i = 0; i < breakableObjects.Count; i++)
        {
            // If the object was destroyed
            if (breakableObjects[i].destroyed)
            {
                destroyedObjects.Add(breakableObjects[i]);
                Debug.Log(breakableObjects[i].ToString() + " was destroyed. Distance from worker " + id + ": " + Vector3.Distance(breakableObjects[i].transform.position, transform.position).ToString());
                // If this worker heard the object being destroyed and is not chasing the raccoon
                if (!chasing && Vector3.Distance(breakableObjects[i].transform.position, transform.position) < hearingRadius)
                {
                    investigating = true;
                    searching = false;
                    idle = false;
                    lastKnownLocation = breakableObjects[i].transform.position;
                    agent.SetDestination(lastKnownLocation);
                    Debug.Log("Worker " + id + " heard object " + breakableObjects[i].ToString() + " being destroyed. Now heading to " + breakableObjects[i].transform.position.ToString() + " to investigate");
                }
            }
            
        }
        
        // Remove all objects that have been destroyed from 'breakableObjects'
        for (int i = 0; i < destroyedObjects.Count; i++)
        {
            breakableObjects.Remove(destroyedObjects[i]);
            Debug.Log(destroyedObjects[i].ToString() + " has been removed");
        }
        destroyedObjects.Clear();

        // For each knockable object on this worker's floor
        for (int i = 0; i < knockableObjects.Count; i++)
        {
            // If the object was knocked over
            if (knockableObjects[i].toppled)
            {
                toppledObjects.Add(knockableObjects[i]);
                Debug.Log(knockableObjects[i].ToString() + " was knocked over. Distance from worker " + id + ": " + Vector3.Distance(knockableObjects[i].transform.position, transform.position).ToString());
                // If this worker heard the object being knocked over and is not chasing the raccoon
                if(!chasing && Vector3.Distance(knockableObjects[i].transform.position, transform.position) < hearingRadius)
                {
                    investigating = true;
                    searching = false;
                    idle = false;
                    lastKnownLocation = knockableObjects[i].transform.position;
                    agent.SetDestination(lastKnownLocation);
                    Debug.Log("Worker " + id + " heard object " + knockableObjects[i].ToString() + " being knocked over. Now heading to " + knockableObjects[i].transform.position.ToString() + " to investigate");
                }
            }
        }

        for (int i = 0; i < toppledObjects.Count; i++)
        {
            knockableObjects.Remove(toppledObjects[i]);
            Debug.Log(toppledObjects[i].ToString() + " has been removed");
        }
        toppledObjects.Clear();

        // The worker will return to his original position if he can't find the raccoon
        if (idle)
        {
            if (System.Math.Abs(transform.position.x - initialPosition.x) <= 2 &&
            System.Math.Abs(transform.position.z - initialPosition.z) <= 2)
            {
                //Debug.Log("In starting position. current position: " + transform.position.ToString() + " initial position: " + initialPosition.ToString());
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialDirection, rotationSpeed);
            }
            else
            {
                //Debug.Log("Can't find Raccoon. Returning to initial position");
                //Debug.Log("Not in starting position. current position: " + transform.position.ToString() + " initial position: " + initialPosition.ToString());
                if (agent.SetDestination(initialPosition))
                {
                    //Debug.Log("Now heading to " + agent.destination.ToString() + ". Initial position is " + initialPosition.ToString());
                }
            }
        }


        if (anim.GetBool("scared"))
            agent.isStopped = true;
        else
            agent.isStopped = false;

        if (anim.GetBool("attack"))
            agent.isStopped = true;
        else
            agent.isStopped = false;
    }

    private void chaseVO()
    {
        // randomize        
        int randIdx = Random.Range(0, workerChaseVO.Length);
        WorkerAudio.PlayOneShot(workerChaseVO[randIdx], 0.8F);
        // Ensures a true OneShot and no repeated sound
        alreadyPlayed = true;
        _timer = 0;

    }

    private void stunVO()
    {
        // randomize        
        int randIdx = Random.Range(0, workerStunVO.Length);
        WorkerAudio.PlayOneShot(workerStunVO[randIdx], 0.5F);
        // Ensures a true OneShot and no repeated sound
        alreadyPlayed = true;
        _timer = 0;

    }

    public void TogglePlay()
    {
        if (pause)
        {
            agent.isStopped = false;
        } else 
        {
            agent.isStopped = true;
        }
        pause = !pause;
        anim.enabled = !anim.enabled;
    }

    private bool inRange(Vector3 target, float range) 
    {
        return System.Math.Abs(transform.position.x - target.x) < range && System.Math.Abs(transform.position.z - target.z) < range;
    }
}
