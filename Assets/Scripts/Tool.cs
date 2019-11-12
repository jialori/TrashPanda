using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Tool : MonoBehaviour
{
    //private GameObject player;
    private RaccoonController playerScript;
    private Rigidbody rb;
    private float timer;                    // Time before this bonus tool expires
    public string toolType;

    protected bool beingCarried = false;

    [Header("Tool Attributes")]
    public float effectOnAttack = 10;
    public float effectOnSpeed = -5;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.instance.Raccoon.GetComponent<RaccoonController>();
        // Once the player completes a task, equip the bonus tool and start its timer
        // timer = 10f;
        // Equip();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (beingCarried)
            {
                UnEquip();
            }
            Destroy(this);
        }
    }

    // The function called by 
    /*
    public void Activate()
    {
        if (!beingCarried)
        {
            Equip();
        } else {
            UnEquip();
        }
    }
    */

    public void Equip() 
    {
        // Called
        rb.isKinematic = true;
        beingCarried = true;
        transform.parent = playerScript.transform;
        transform.localPosition = playerScript.transform.forward;
    	playerScript.AddStrengthModifier(effectOnAttack, effectOnSpeed);
    }

    public void UnEquip()
    {
	    rb.isKinematic = false;
	    beingCarried = false;
	    Vector3 currLoc = transform.position;
	    transform.parent = null;
	    transform.position = currLoc;
	    playerScript.RemoveStrengthModifier(effectOnAttack, effectOnSpeed);	
    }

}
