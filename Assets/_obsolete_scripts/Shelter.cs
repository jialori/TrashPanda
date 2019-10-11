using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    public GameObject player;               // Reference to player object
    public RaccoonController RacScript;     // Reference to racoon controller

    private int children;               // Number of racoon children in the shelter
    private int m_numFood;              // Food currently stored in shelter (maybe children should eat at set intervals?)
    private bool isTouching;                        // Used to determine if the player is close enough to the shelter for interaction

    void Start()
    {
        children = 3;
        m_numFood = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("touch enter");
            isTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("touch leave");
            isTouching = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E down");
            if (isTouching)
            {
                IncreaseFood();
            }
        }
    }

    private bool IncreaseFood()
    {
        Debug.Log("Adding food to shelter");
        Debug.Log(m_numFood);
        if (RacScript.DecreaseFood())
        {
            Debug.Log("Increasing amount");
            m_numFood++;
            return true;
        }
        else
        {
            return false;
        }
    }

    
}
