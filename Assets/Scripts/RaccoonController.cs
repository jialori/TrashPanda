using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    public float speed = 20.0f;
    public int food = 0;
    public int maxFood = 10;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // float moveHorizontal = Input.GetAxis("Horizontal");
        // float moveVertical = Input.GetAxis("Vertical");

        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // rb.AddForce(movement * speed);

        float MoveHorizontal = speed * Input.GetAxis("Horizontal");
        float MoveVertical = speed * Input.GetAxis("Vertical");
        Vector3 MovePlayer = new Vector3(MoveHorizontal, 0.0f, MoveVertical);
        rb.velocity = MovePlayer;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Add code to begin interaction here
        }
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
        return true;
    }
}
