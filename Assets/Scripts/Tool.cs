﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Tool : MonoBehaviour
{
    //private GameObject player;
    private RaccoonController playerScript;
    private Rigidbody rb;
    private static float timerLength = 20f; // Total amount of time
    private float timer;                    // Time before this bonus tool expires
    public string toolType;

    protected bool beingCarried = false;

    [Header("Tool Attributes")]
    public float effectOnAttack = 10;
    private void Start()
    {
        Debug.Log("tool start");
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.instance.Raccoon.GetComponent<RaccoonController>();
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
            Destroy(this.gameObject);
        }
    }

    public void Equip() 
    {
        // Called
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.instance.Raccoon.GetComponent<RaccoonController>();
        rb.isKinematic = true;
        beingCarried = true;
        transform.parent = playerScript.transform;
        transform.localPosition = playerScript.transform.forward;
        var offset = playerScript.transform.right * -0.5f; 
        offset.y = 0;
        transform.localPosition += offset;
    	playerScript.AddStrengthModifier(effectOnAttack, 0);

        timer = timerLength;
        ObjectManager.instance.EquipTool(this);
    }

    public void UnEquip()
    {
	    rb.isKinematic = false;
	    beingCarried = false;
	    Vector3 currLoc = transform.position;
	    transform.parent = null;
	    transform.position = currLoc;
	    playerScript.RemoveStrengthModifier(effectOnAttack, 0);	
        ObjectManager.instance.UnequipTool(this);
    }

    public void AddTime()
    {
        timer += timerLength;
    }
}