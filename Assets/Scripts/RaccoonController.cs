using UnityEngine;
using Util;

public class RaccoonController : MonoBehaviour
{
    private CharacterController characterController;
    public CharacterController charController { get => characterController; }
    private Animator animator;

    [SerializeField] private Transform cam;
    [SerializeField] private CameraRotator camController;

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

    private Vector3 movementVector;

    // For interaction with Breakable and Knockable
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private int breakableMask;
    private int knockableMask;

    private bool isOnUpStair = false;
    private bool isOnDownStair = false;
    public bool isStunned = false;
    public float stunTimer = 3.0f;

    private bool pause = false;

    void Start()
    {
        AudioManager.instance.Play("ThemeSong");
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // For interaction with breakable and knockable
        raycastPaddedDist = characterController.radius + raycastPadding;
        breakableMask = 1 << LayerMask.NameToLayer(breakableMaskName);
        knockableMask = 1 << LayerMask.NameToLayer(knockableMaskName);

        // Set Raccoon's attack power
        // attackPower = 1;
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

        // If the raccoon is stunned, she cannot move, jump or break objects
        if (!isStunned)
        {
            // Movement
            movementVector = (camForward * Controller.GetYAxis() + camRight * Controller.GetXAxis()) * movementSpeed;

            // Jump
            if (characterController.isGrounded)
            {
                movementVector.y = 0;
                if (Controller.GetA())
                {
                    movementVector.y = jumpPower;
                    animator.SetTrigger("jumped");
                }
            }
            else
            {
                movementVector.y = prevY;
            }

            movementVector.y -= gravity * Time.deltaTime;
            // Debug.Log("movementVector = " + movementVector);
            characterController.Move(movementVector * Time.deltaTime);
        }
        else if (isStunned)
        {
            if (stunTimer > 0)
            {
                stunTimer -= Time.deltaTime;
            }
            else
            {
                isStunned = false;
                stunTimer = 3.0f;
            }
        }

        // Rotation
        if (camController != null)
        {
            float lookS = camController.position.lookSmooth;
            Vector3 lookDir = transform.position - cam.position;
            lookDir.y = 0;
            Quaternion tarRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Lerp(Quaternion.identity, tarRotation, lookS * Time.deltaTime);
        }
        else
        {
            Debug.Log("InspectorWarning: camController in Raccoon is Not correct. Please assign Camera to it.");
        }

        // Animation
    	// animator.SetBool("isMoving", true);
        if (!isStunned && !Controller.GetA() && (Controller.GetXAxis() != 0.0f || Controller.GetYAxis() != 0.0f))
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


    public void AddStrengthModifier(float effectOnAttack, float effectOnSpeed)
    {
        attackPower += effectOnAttack;
        movementSpeed += effectOnSpeed;
        Debug.Log("Attack has changed:" + attackPower);
        Debug.Log("Speed has changed:" + movementSpeed);
    }

    public void RemoveStrengthModifier(float effectOnAttack, float effectOnSpeed)
    {
        attackPower -= effectOnAttack;
        movementSpeed -= effectOnSpeed;
        Debug.Log("Attack has changed:" + attackPower);
        Debug.Log("Speed has changed:" + movementSpeed);
    }

    public void BreakObjectsNearby()
    {
        RaycastHit hit;
        // Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms.
        Vector3 p1 = transform.position + Vector3.up * 0.01f;
        Vector3 p2 = p1 + Vector3.up * characterController.height;
        // Check around the character in 360 degree
        for (int i = 0; i < 360; i += radiusStep)
        {
            // Check if anything with the breakable layer touches this object
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist, breakableMask))
            {
                Breakable breakable = hit.collider.gameObject.GetComponent<Breakable>() as Breakable;
                if ((breakable != null) && (Time.time > nextHit))
                {
                    nextHit = Time.time + hitRate;
                    breakable.trigger(attackPower);
                }
            }
        }
    }

    public void UseStairs(bool up)
    {
        characterController.enabled = false;
        if (up) characterController.transform.position += new Vector3(0, 8.5f, 0);
        else characterController.transform.position -= new Vector3(0, 8, 0);
        characterController.enabled = true;
    }

    public void TogglePlay()
    {
        pause = !pause;
        animator.enabled = !animator.enabled;
    }

}