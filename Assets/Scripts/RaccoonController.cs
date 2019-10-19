using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Transform cam;
    [SerializeField] private bool useController = true;

    [Header("Character Stats")]
    [SerializeField] private float attackPower = 1;
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float jumpPower = 15;
    [SerializeField] private float gravity = 40;
    [SerializeField] private float pushPower = 12;

    public static float score = 0;
    private Vector3 movementVector;

    // For interaction with Breakable and Knockable
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private int breakableMask;
    private int knockableMask;
    // For interaction with Breakable
    private float raycastPaddedDist;
    private float raycastPadding = 0.2f;
    private int radiusStep = 36; // how many degree does each raycast check skips, dcrease if want more accuracy
    private float nextHit;
    public float hitRate = 0.5f;

    void Start()
    {
        AudioManager.instance.Play("ThemeSong");
        characterController = GetComponent<CharacterController>();

        // For interaction with breakable and knockable
        raycastPaddedDist = characterController.radius + raycastPadding;
        breakableMask = 1 << LayerMask.NameToLayer(breakableMaskName);
        knockableMask = 1 << LayerMask.NameToLayer(knockableMaskName);
        
        // Set Raccoon's attack power
        // attackPower = 1;
        // score = 0;
    }

    void Update()
    {
        // Adjust movement for camera angle
        var camForward = cam.forward;
        var camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        var prevY = movementVector.y;

        // Movement
        movementVector = (camForward * GetYAxis() + camRight * GetXAxis()) * movementSpeed;

        // Jump
        if (characterController.isGrounded)
        {
            movementVector.y = 0;
            if (GetJump())
            {
                movementVector.y = jumpPower;
            }
        }
        else
        {
            movementVector.y = prevY;
        }

        movementVector.y -= gravity * Time.deltaTime;
        //Debug.Log("movementVector = " + movementVector);
        characterController.Move(movementVector * Time.deltaTime);

        // Break Breakable objects
        if (Input.GetButtonDown("B"))
        {
            BreakObjectsNearby();
        }
    }

    // On collision, knock Knockable objects over
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Knock knockable objects
        Knockable knockable = hit.gameObject.GetComponent("Knockable") as Knockable;
        if (knockable != null)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3 pushForce = pushDir * pushPower;
            knockable.trigger(pushForce);
        }
    }


    private float GetXAxis()
    {
        if (useController)
        {
            return Input.GetAxis("LeftJoystickX");
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    private float GetYAxis()
    {
        if (useController)
        {
            return -Input.GetAxis("LeftJoystickY");
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    private bool GetJump()
    {
        if (useController)
        {
            return Input.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    private Vector3 CameraRelativeFlatten(Vector3 input, Vector3 localUp)
    {
        // If this script is on your camera object, you can use this.transform instead.

        // The first part creates a rotation looking into the ground, with
        // "up" matching the camera's look direction as closely as it can. 
        // The second part rotates this 90 degrees, so "forward" input matches 
        // the camera's look direction as closely as it can in the horizontal plane.
        Quaternion flatten = Quaternion.LookRotation(-localUp,
            this.cam.forward
          ) *
          Quaternion.Euler(Vector3.right * -90f);

        // Now we rotate our input vector into this frame of reference
        return flatten * input;
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


}