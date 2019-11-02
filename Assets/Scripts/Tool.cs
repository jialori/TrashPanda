using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Tool : MonoBehaviour
{
    public GameObject player;
    private RaccoonController playerScript;
    private Rigidbody rb;
    public string toolType;

    private bool beingCarried = false;

    [Header("Tool Attributes")]
    public float effectOnAttack = 10;
    public float effectOnSpeed = -5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.instance.Raccoon.GetComponent<RaccoonController>();
    }

    // The function called by 
    public void Activate()
    {
        if (!beingCarried)
        {
            Equip();
        } else {
            UnEquip();
        }
    }

    public void Equip() 
    {
        // Called
        rb.isKinematic = true;
        beingCarried = true;
        transform.parent = player.transform;
        transform.localPosition = player.transform.forward;
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
