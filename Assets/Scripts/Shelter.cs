using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    private int children = 3;               // Number of racoon children in the shelter
    private int m_numFood = 0;              // Food currently stored in shelter (maybe children should eat at set intervals?)
    public GameObject player;               // Reference to player object
    public RaccoonController RacScript;     // Reference to racoon controller
    bool isTouching;                        // Used to determine if the player is close enough to the shelter for interaction

    // Start is called before the first frame update
    void Start()
    {
        
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
