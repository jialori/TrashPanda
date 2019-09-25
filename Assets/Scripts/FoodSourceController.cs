using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO (do after design decisions, before more technical implementations): 
//			Refactor script to inherent FoodSourceController so 
// 			that it notifies "player", but can perform other unique behaviour 
//			at the same time
public class FoodSourceController : MonoBehaviour
{
    protected int m_NumFood = 5; //TODO: should this be public
    public GameObject player;
    public RaccoonController RacScript;
    float TimeInterval;
    bool isTouching;
    bool keyHeld;
    bool gettingFood;

    private void Start()
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
        // Give it some berries?
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E down");
            //Debug.Log(isTouching);
            if (isTouching)
                DecreaseFood();
        }
        //if (Input.GetKeyUp(KeyCode.E))
        //{
            
        //    Debug.Log("E up");
        //}
    }

    private bool DecreaseFood()
    {
        Debug.Log("Decreasing food");
        Debug.Log(m_NumFood);
        if (m_NumFood > 0 && RacScript.IncreaseFood())
        {
            Debug.Log("Decrease amount");
            m_NumFood--;
            return true;
        }
        return false;
    }
}
