using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    private CharacterController characterController;
    public CharacterController Controller { get => characterController; }
    [SerializeField] private Transform cam;

    [Header("Character Stats")]
    [SerializeField] private float attackPower = 1;
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float jumpPower = 15;
    [SerializeField] private float gravity = 40;
    [SerializeField] private float pushPower = 12;

    [SerializeField] private float rotateSpeed = 5;

    // For interaction with Breakable
    [Header("Raytracing (Breakable)")]
    [SerializeField] private float raycastPaddedDist;
    [SerializeField] private float raycastPadding = 21.2f;
    [SerializeField] private int radiusStep = 36; // how many degree does each raycast check skips, dcrease if want more accuracy

    [Header("Hit Frequency (Breakable)")]
    [SerializeField] private float nextHit;
    [SerializeField] public float hitRate = 0.5f;

    private Vector3 movementVector;

    // For interaction with Breakable and Knockable
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private int breakableMask;
    private int knockableMask;

    private bool isOnUpStair = false;
    private bool isOnDownStair = false;

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
    }

    void Update()
    {
        // Move up or down stairs
        if ((isOnUpStair || isOnDownStair) && GetInteract())
        {
            characterController.enabled = false;
            if (isOnUpStair) characterController.transform.position += new Vector3(0, 8.5f, 0);
            if (isOnDownStair) characterController.transform.position -= new Vector3(0, 8, 0);
            characterController.enabled = true;
            return;
        }

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

        // Rotation
        transform.Rotate(new Vector3(0, GetCamXAxis() * rotateSpeed, 0));

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
        // Debug.Log("movementVector = " + movementVector);
        characterController.Move(movementVector * Time.deltaTime);

        // Break Breakable objects
        if (GetInteract())
        {
            BreakObjectsNearby();
        }
    }

    // On collision, knock Knockable objects over and mark stair usage
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        this.isOnUpStair = false;
        this.isOnDownStair = false;
        Stair stair = hit.gameObject.GetComponent("Stair") as Stair;
        if (stair != null)
        {
            // Debug.Log("stair hit");
            if (hit.gameObject.tag == "UpStair") this.isOnUpStair = true;
            if (hit.gameObject.tag == "DownStair") this.isOnDownStair = true;
        }

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

    private float GetXAxis()
    {
        if (GameManager.instance.UseController)
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
        if (GameManager.instance.UseController)
        {
            return -Input.GetAxis("LeftJoystickY");
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    private float GetCamXAxis()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetAxis("RightJoystickX");
        }
        else
        {
            return Input.GetAxis("Mouse X");
        }
    }
    
    private bool GetJump()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    private bool GetInteract()
    {
        if (GameManager.instance.UseController) 
        {
            return Input.GetButtonDown("B");
        } else {
            return Input.GetKeyDown("e");
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
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist))
            {
                Debug.Log("[RaccoonController] Hit");
                Debug.Log("p1: {p1} p2: {p2} i: {i}");
            }
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

