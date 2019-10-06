using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    [SerializeField] private AudioClip itemCollectSound;
    [SerializeField] private bool useController = true;
    [SerializeField] private int food = 0;
    [SerializeField] private int maxFood = 10;

    [SerializeField] private Transform cam;

    private Vector3 movementVector;

    private CharacterController characterController;

    private float movementSpeed = 10;
    private float jumpPower = 15;
    private float gravity = 40;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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

        // movement
        movementVector = (-camForward * GetYAxis() + camRight * GetXAxis()) * movementSpeed;
        
        // Jump
        if (characterController.isGrounded)
        {
            movementVector.y = 0;

            if (Input.GetButtonDown("A"))
            {
                movementVector.y = jumpPower;
            }
        } else {
            movementVector.y = prevY;
        }

        movementVector.y -= gravity * Time.deltaTime;
        Debug.Log("movementVector = " + movementVector);

        characterController.Move(movementVector * Time.deltaTime);
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
            return Input.GetAxis("LeftJoystickY");
        } 
        else 
        {
            return Input.GetAxis("Vertial");
        }
    }

    private Vector3 CameraRelativeFlatten(Vector3 input, Vector3 localUp)
    {
        // If this script is on your camera object, you can use this.transform instead.

        // The first part creates a rotation looking into the ground, with
        // "up" matching the camera's look direction as closely as it can. 
        // The second part rotates this 90 degrees, so "forward" input matches 
        // the camera's look direction as closely as it can in the horizontal plane.
        Quaternion flatten = Quaternion.LookRotation(
                                            -localUp, 
                                            this.cam.forward
                                    )
                                        * Quaternion.Euler(Vector3.right * -90f);

        // Now we rotate our input vector into this frame of reference
        return flatten * input;
    }

    public int GetFood()
    {
        return food;
    }

    public int GetMaxFood()
    {
        return maxFood;
    }

    public bool IncreaseFood()
    {
        if (food >= maxFood)
        {
            Debug.Log("At max food");
            return false;
        }

        Debug.Log("Got a food");
        food++;
        AudioManager.instance.Play("ItemCollect");
        return true;
    }

    public bool DecreaseFood()
    {
        if (food <= 0)
        {
            Debug.Log("Have no food");
            return false;
        }

        Debug.Log("Gave a food");
        food--;
        return true;
    }
}