using UnityEngine;
using Util;
using System.Collections;

public class RaccoonController : MonoBehaviour
{
    public CameraShaker camShaker;
    private CharacterController characterController;
    public CharacterController charController { get => characterController; }
    private Animator animator;

    [SerializeField] private Transform cam;
    [SerializeField] private CameraRotator camController;

    [Tooltip("How many degrees the raccoon can turn per 1 second.")]
    public float turningRate = 360f;

    [Header("Character Stats")]
    [SerializeField] private float attackPower = 1;
    public float AttackPower { get => attackPower; }

    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float jumpPower = 15;
    [SerializeField] private float gravity = 40;
    [SerializeField] private float pushPower = 12;

    // For interaction with Breakable
    [Header("Raytracing (Breakable)")]
    [SerializeField] private float raycastPaddedDist;
    [SerializeField] private float raycastPadding = 21.2f;
    [SerializeField] private int radiusStep = 36; // how many degree does each raycast check skips, dcrease if want more accuracy

    [Header("Hit Frequency (Breakable)")]
    [SerializeField] public float nextHit;
    [SerializeField] public float hitRate = 0.5f;
    public float HitRate { get => hitRate; }

    public int level;

    private Vector3 movementVector;
    private ParticleSystem stunEffect;

    private bool isOnUpStair = false;
    private bool isOnDownStair = false;
    public bool isStunned = false;
    public float stunTimer = 3.0f;
    public bool isFrozen = false;
    private bool pause = true;
    private Vector3 initPosition;
    private Quaternion initRotation;

    public AudioSource[] raccoonSounds;

    public AudioSource sfx1;
    public AudioSource sfx2;

    public AudioClip stunnedSFX;

    void Awake()
    {
        if (camController == null)
        {
            Debug.Log("InspectorWarning: camController in Raccoon is missing. Please assign Camera to it.");
        }

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // For interaction with breakable and knockable
        raycastPaddedDist = characterController.radius + raycastPadding;
        stunEffect = GetComponentInChildren<ParticleSystem>(true);
        stunEffect.Stop();
        var sm = stunEffect.main;
        sm.simulationSpeed = 2f;
        animator.enabled = false;

    }

    void Start()
    {
        raccoonSounds = GetComponents<AudioSource>();
        sfx1 = raccoonSounds[0];
        sfx2 = raccoonSounds[1];

        initPosition = transform.position;
        initRotation = transform.rotation;
        Debug.Log(initPosition);
    }

    void Update()
    {
    	if (pause) return;

        // Adjust movement for camera angle
        var camForward = cam.forward;
        var camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        var prevY = movementVector.y;

        if (transform.position.y < 7)
        {
            level = 1;
        }
        else if (7 <= transform.position.y && transform.position.y < 14)
        {
            level = 2;
        }
        else if (14 <= transform.position.y && transform.position.y < 21)
        {
            level = 3;
        }
        else if (21 <= transform.position.y && transform.position.y < 28)
        {
            level = 4;
        }
        else
        {
            level = 5;
        }
        // Debug.Log("RaccoonController: position.x = " + transform.position.x.ToString() + " position.y = " + transform.position.y.ToString() + " position.z = " + transform.position.z.ToString());

        float moveX = Controller.GetXAxis();
        float moveY = Controller.GetYAxis();
        // If the raccoon is stunned, she cannot move, jump or break objects
        if (!isStunned && !isFrozen)
        {
            // Movement
            movementVector = (camForward * moveY + camRight * moveX) * movementSpeed;

            // Rotation
            if ((moveX != 0 || moveY != 0))
            {
                // - Turn towards camera
                // Vector3 lookDir = transform.position - cam.position;
                // lookDir.y = 0;
                // Quaternion tarRotation = Quaternion.LookRotation(lookDir);
                // transform.rotation = Quaternion.Lerp(Quaternion.identity, tarRotation, turningRate * Time.deltaTime);
            
                // - Turn towards direction of moving (except when walking backwards)
                // int sign = (moveY >= 0) ? 1 : -1;
                // Quaternion tarRotation = Quaternion.LookRotation(sign * movementVector);
                // if (Quaternion.Angle(transform.rotation, tarRotation) >= 175) 
                // {
                //     // To prevent raccoon from turning through the backside (quaternion default behavior)
                //     transform.rotation = Quaternion.RotateTowards(transform.rotation, tarRotation, -0.1f * turningRate * Time.deltaTime);
                // }
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, tarRotation, turningRate * Time.deltaTime);
                
                // - Turn towards direction of moving
                Quaternion tarRotation = Quaternion.LookRotation(movementVector);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, tarRotation, turningRate * Time.deltaTime);
            }

            // Jump
            if (characterController.isGrounded)
            {
                movementVector.y = 0;
                if (Controller.GetA())
                {
                    movementVector.y = jumpPower;
                    animator.SetTrigger("jumped");
                }

                animator.SetBool("isGrounded", true);

            }
            else
            {
                movementVector.y = prevY;
            
                animator.SetBool("isGrounded", false);

            }

            movementVector.y -= gravity * Time.deltaTime;
            Debug.Log("movementVector = " + movementVector);
            characterController.Move(movementVector * Time.deltaTime);
        }
        else if (isStunned)
        {
            //Debug.Log("Raccoon is stunned");
            if (stunTimer > 0)
            {
                stunTimer -= Time.deltaTime;
            }
            else
            {
                isStunned = false;
                stunTimer = 3.0f;
                stunEffect.Stop();
            }

            // raccoon is still affected by gravity when stunned
            movementVector = new Vector3(0, 0, 0);
            movementVector.y = - gravity * Time.deltaTime;
            characterController.Move(movementVector * Time.deltaTime);
        }

        // Animation
        if (!isStunned && !Controller.GetA() && (moveX != 0.0f || moveY != 0.0f))
        {
        	animator.SetBool("isMoving", true);
        } else
        {
        	animator.SetBool("isMoving", false);	
        }

    }

    // On collision, knock Knockable objects over and mark stair usage
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Knock over a knock-over-able objects
        Knockable knockable = hit.gameObject.GetComponent("Knockable") as Knockable;
        if (knockable != null)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3 pushForce = pushDir * pushPower;
            knockable.trigger(pushForce);
        }
    }


    public void UseStairs(bool up)
    {
        characterController.enabled = false;
        if (up) characterController.transform.position += new Vector3(0, 8.5f, 0);
        else characterController.transform.position -= new Vector3(0, 8, 0);

        // Set to x-z position tight in front of door
            characterController.transform.position.x = 27.41;
        characterController.transform.position.z = -16.53;

        characterController.enabled = true;
        characterController.transform.eulerAngles = new Vector3(0, -90, 0);

    }

    public void Pause()
    {
        pause = true;
        animator.enabled = false;        
    }

    public void UnPause()
    {
        pause = false;
        animator.enabled = true;        
    }

    public void TogglePause()
    {
        // Debug.Log("Toggled");
        pause = !pause;
        animator.enabled = !animator.enabled;
    }

    public void StunRaccoon(Vector3 stunFrom)
    {
        isStunned = true;
        camShaker.ShakeCamera();
        StartCoroutine(Stun(stunFrom));
    }

    // public void Reset()
    // {
    //     Debug.Log(transform.position);
    //     transform.position = initPosition;
    //     transform.rotation = initRotation;
    //     Debug.Log(transform.position);
    // }

    private IEnumerator Stun(Vector3 stunFrom)
    {
        stunEffect.Play();
        sfx2.PlayOneShot(stunnedSFX, 0.3f);
        yield return new WaitForSeconds(1);
    }
}