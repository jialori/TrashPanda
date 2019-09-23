using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    [SerializeField] private float speed = 80;
    [SerializeField] private int food = 0;
    [SerializeField] private int maxFood = 10;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        float moveHorizontal = speed * Input.GetAxis("Horizontal");
        float moveVertical = speed * Input.GetAxis("Vertical");
        Vector3 movePlayer = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movePlayer;
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
