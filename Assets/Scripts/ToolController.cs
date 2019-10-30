using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    public GameObject player;
    private RaccoonController playerScript;
    private Rigidbody rb;
    public string toolType;

    private bool hasPlayer = false;
    private bool beingCarried = false;
    private static bool handFree = true;
    public static ToolController toolInHand = null;

    [Header("Tool Attributes")]
    public float effectOnAttack = 10;
    public float effectOnSpeed = -5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // player = GameManager.instance.Raccoon.gameObject;
        playerScript = player.GetComponent<RaccoonController>();
    }

    void Update()
    {
        float dis = Vector3.Distance(this.transform.position, player.transform.position);
        hasPlayer = (dis <= 3f) ? true : false;
        if (Input.GetButtonDown("X") && hasPlayer && handFree)
        {
            Equip();
        }

	    if (beingCarried && Input.GetButtonDown("Y"))
	    {
	        UnEquip();
        }
    }


    public void Equip() 
    {
        rb.isKinematic = true;
        beingCarried = true;
        handFree = false;
        toolInHand = this;
        transform.parent = player.transform;
        transform.localPosition = player.transform.forward;
    	playerScript.AddStrengthModifier(effectOnAttack, effectOnSpeed);
    }

    public void UnEquip()
    {
	    rb.isKinematic = false;
	    beingCarried = false;
	    handFree = true;
	    toolInHand = null;
	    Vector3 currLoc = transform.position;
	    transform.parent = null;
	    transform.position = currLoc;
	    playerScript.RemoveStrengthModifier(effectOnAttack, effectOnSpeed);	
    }

}
