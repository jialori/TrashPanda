using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{

    [SerializeField] private float maxAngle = 30.0f;                    // Field of view of this worker
    [SerializeField] private float maxRadius = 15.0f;                   // The farthest distance that this worker can see
    [SerializeField] private float hearingRadius = 40.0f;               // The farthest distance that this worker can hear
    [SerializeField] private int level;                                 // The floor this worker is on 
    private float rotationSpeed = 5.0f;                                 // How fast this worker rotates
    private float attackCooldown = 10.0f;                               // The cooldown timer for the worker's stun attack
    Quaternion initialDirection;                                        // Direction this worker initially faces. Will rotate to face this direction after returning to 'initialPosition'

    List<Breakable> breakableObjects;                                   // List of breakable objects on this worker's floor
    List<Breakable> destroyedObjects = new List<Breakable>();           // List of destroyed objects. Used to remove objects from 'breakableObjects'

    List<Knockable> knockableObjects;                                   // List of knockable objects on this worker's floor
    List<Knockable> toppledObjects = new List<Knockable>();             // List of knocked over objects. Used to remove objects from 'knockableObjects'

    Vector3 initialPosition;                // Starting position of this worker. Will return here after losing sight of raccoon
    public Transform target;                // Worker target to be chased (will always be the raccoon)
    public Vector3 lastKnownLocation;       // Location where this worker last saw the raccoon
    NavMeshAgent agent;                     // Pathfinding AI
    CentralHumanController CHC;             // Reference to the Central Human Controller

    public bool seesRaccoon = false;        // Flag determining whether this worker can see the raccoon or not
    private bool canAttack = true;          // Flag determining whether this worker can attack
    private bool chasing = false;           // Worker status: The worker knows where the raccoon is and is currently chasing her
    //private bool searching = false;         // Worker status: The raccoon has escaped the worker's sight and the worker is looking for her
    private bool investigating = false;     // Worker status: A noise has caught this worker's attention and the worker is investigating
    private bool idle = true;               // Worker status: The worker does not know where the raccoon is and is not looking for her

    AudioSource WorkerAudio;                // Audiosource files and script
    public AudioClip[] workerChaseVO;       // Chasing VO
    public AudioClip[] workerStunVO;        // Chasing VO
    public float replayInterval;            // Time till replay is ready
    private float _timer = 0;
    private bool alreadyPlayed = false;     // Helps with OneShot trigger to only have one instance of sound    

    // Intermediate variables
    NavMeshPath p;
    [SerializeField] private string id;                     // This worker's identifier. Used for debugging

    // Animation
    private Animator anim;

    // For Play/Pause toggle 
    private bool pause = false;

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

    // Determine if the worker and the player are on the same floor
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
            //Debug.Log(1);
            //Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);

            // If the player is close enough to the worker to be seen
            if (Vector3.Distance(checkingObject.position, target.position) < maxRadius)
            {
                //Debug.Log(2);

                // If the player and worker are on the same floor
                if (onSameFloor(target))
                {
                    //Debug.Log(3);
                    // If the worker can directly see the player (i.e. line of sight is not blocked by wall or bush)
                    if (!nav.Raycast(target.position, out _))
                    {
                        Debug.Log("HumanController: Worker level = " + level.ToString() + "Raccoon level = " 
                            + target.GetComponent<RaccoonController>().level.ToString());
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
        agent.stoppingDistance = 1f;
        p = new NavMeshPath();
        initialPosition = transform.position;
        initialDirection = transform.rotation;

        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("idle", true);

        //Audio Component
        WorkerAudio = GetComponent<AudioSource>();
        workerChaseVO = Resources.LoadAll<AudioClip>("Audio/ChaseVO");
        workerStunVO = Resources.LoadAll<AudioClip>("Audio/StunVO");
    }

    void Update() 
    {
        if (pause) return;

        // Register worker to CHC; code cannot be placed into Start because Gamemanager's CHC might not have been initialized yet
        // there are better ways to do this
        if (CHC == null) 
        {
            // Register itself at GameManager
            CHC = GameManager.instance.CHC;
            CHC.humans.Add(this);

            breakableObjects = CHC?.allObjects[level - 1].Item1;
            knockableObjects = CHC?.allObjects[level - 1].Item2;
            return;
        }

        // I don't know who put this here but get rid of it!!! It messes up the workers when the raccoon leaves the floor!!!
        //if (!onSameFloor(target))
        //    return;

        _timer += Time.deltaTime;
        if (alreadyPlayed && _timer > replayInterval)
        {
            alreadyPlayed = false;
        }

        seesRaccoon = inFOV(agent, transform, target, maxAngle, maxRadius);
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
            //searching = false;
            investigating = false;
            idle = false;
            anim.SetBool("idle", false);

            //agent.ResetPath();
            agent.SetDestination(lastKnownLocation);

            //Audio trigger for sighting Raccoon
            if (!seesRaccoon && !alreadyPlayed)
            {
                chaseVO();
            }

            // Animation
            anim.SetBool("chasing", true);
            anim.SetBool("scared", false);
        }
        //Debug.Log("Distance(x): " + System.Math.Abs(transform.position.x - lastKnownLocation.x).ToString() + ", Distance(z): " + System.Math.Abs(transform.position.z - lastKnownLocation.z).ToString());
        //Debug.Log("seesRaccoon: " + seesRaccoon);

        /*
        // If the raccoon leaves the nav mesh, the worker will return to the 'idle' state
        if (!agent.pathPending)
        {
            Debug.Log("Raccoon not in nav mesh");
            chasing = false;
            searching = false;
            idle = true;
            anim.SetBool("idle", true);
        }
        */

        // The worker will stop if he is at 'lastKnownLocation' and can't see the raccoon
        if (!seesRaccoon && System.Math.Abs(transform.position.x - lastKnownLocation.x) <= 4 &&
            System.Math.Abs(transform.position.z - lastKnownLocation.z) <= 4)
        {
            //Debug.Log("Lost Raccoon");
            // If the raccoon is nearby, the worker will still chase her
            if (System.Math.Abs(transform.position.x - target.position.x) <= 4 &&
                System.Math.Abs(transform.position.z - target.position.z) <= 4)
            {
                transform.LookAt(target);
            }
            // Otherwise, the worker will go idle
            else
            {
                chasing = false;
                investigating = false;
                idle = true;
                agent.ResetPath();

                // Animation
                anim.SetBool("chasing", false);
            }
        }

        if (seesRaccoon && System.Math.Abs(transform.position.x - lastKnownLocation.x) <= 2 &&
            System.Math.Abs(transform.position.z - lastKnownLocation.z) <= 2 && canAttack)
        {
            // Stun attack here
            //Debug.Log("stun attack used");
            target.GetComponent<RaccoonController>().StunRaccoon();
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

        /*
        // The worker will turn to his left and right in case the raccoon is beside him
        if (searching && !chasing && !idle)
        {
            
            Debug.Log("Searching for Raccoon");
            agent.isStopped = true;
            transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f), Time.deltaTime * rotationSpeed);
            transform.Rotate(new Vector3(0.0f, 270.0f, 0.0f), Time.deltaTime * rotationSpeed);
            transform.Rotate(new Vector3(0.0f, 270.0f, 0.0f), Time.deltaTime * rotationSpeed);
            agent.isStopped = false;
            
            searching = false;
            idle = true;
            anim.SetBool("idle", true);
        }
        */

        // For each breakable object on this worker's floor
        for (int i = 0; i < breakableObjects.Count; i++)
        {
            // If the object was destroyed
            if (breakableObjects[i].destroyed)
            {
                destroyedObjects.Add(breakableObjects[i]);
                //Debug.Log(breakableObjects[i].ToString() + " was destroyed. Distance from worker " + id + ": " + Vector3.Distance(breakableObjects[i].transform.position, transform.position).ToString());
                // If this worker heard the object being destroyed and is not chasing the raccoon
                if (!chasing && Vector3.Distance(breakableObjects[i].transform.position, transform.position) < hearingRadius)
                {
                    investigating = true;
                    //searching = false;
                    idle = false;
                    anim.SetBool("idle", false);
                    lastKnownLocation = breakableObjects[i].transform.position;
                    agent.SetDestination(lastKnownLocation);
                    //Debug.Log("Worker " + id + " heard object " + breakableObjects[i].ToString() + " being destroyed. Now heading to " + breakableObjects[i].transform.position.ToString() + " to investigate");
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
            if (knockableObjects[i] && knockableObjects[i].toppled)
            {
                toppledObjects.Add(knockableObjects[i]);
                // Debug.Log(knockableObjects[i].ToString() + " was knocked over. Distance from worker " + id + ": " + Vector3.Distance(knockableObjects[i].transform.position, transform.position).ToString());
                // If this worker heard the object being knocked over and is not chasing the raccoon
                if (!chasing && Vector3.Distance(knockableObjects[i].transform.position, transform.position) < hearingRadius)
                {
                    investigating = true;
                    //searching = false;
                    idle = false;
                    anim.SetBool("idle", false);
                    lastKnownLocation = knockableObjects[i].transform.position;
                    agent.SetDestination(lastKnownLocation);
                    // Debug.Log("Worker " + id + " heard object " + knockableObjects[i].ToString() + " being knocked over. Now heading to " + knockableObjects[i].transform.position.ToString() + " to investigate");
                }
            }
        }

        for (int i = 0; i < toppledObjects.Count; i++)
        {
            knockableObjects.Remove(toppledObjects[i]);
            // Debug.Log(toppledObjects[i].ToString() + " has been removed");
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
                anim.SetBool("idle", true);
            }
            else if (transform.position != initialPosition)
            {
                //Debug.Log("Can't find Raccoon. Returning to initial position");
                if (agent.SetDestination(initialPosition))
                {
                    anim.SetBool("idle", false);
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

    public void chaseVO()
    {
        // randomize        
        int randIdx = Random.Range(0, workerChaseVO.Length);
        WorkerAudio.PlayOneShot(workerChaseVO[randIdx], 0.5F);
        // Ensures a true OneShot and no repeated sound
        alreadyPlayed = true;
        _timer = 0;

    }

    public void stunVO()
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
        agent.isStopped = pause ? false : true;
        pause = !pause;
        anim.enabled = !anim.enabled;
    }
}
