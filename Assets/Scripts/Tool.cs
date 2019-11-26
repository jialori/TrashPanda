using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Tool : MonoBehaviour
{
    //private GameObject player;
    private RaccoonController playerScript;
    private Rigidbody rb;
    private static float timerLength = 10f; // Total amount of time
    private float timer;                    // Time before this bonus tool expires
    public string toolType;
    private TrashManiaDisplay trashManiaDisplay;

    public AudioSource SFX;
    public AudioClip ToolGetSFX;

    protected bool beingCarried = false;

    // public float effectOnAttack = 10;

    
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

    public void Equip(TrashManiaDisplay display) 
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
    	// playerScript.AddStrengthModifier(effectOnAttack, 0);

        timer = timerLength;
        Debug.Log(ObjectManager.instance);
        ObjectManager.instance.EquipTool(this);
        this.trashManiaDisplay = display;
        trashManiaDisplay.gameObject.SetActive(true);
        trashManiaDisplay.Enable(this);
    }

    public void UnEquip()
    {
	    rb.isKinematic = false;
	    beingCarried = false;
	    Vector3 currLoc = transform.position;
	    transform.parent = null;
	    transform.position = currLoc;
	    // playerScript.RemoveStrengthModifier(effectOnAttack, 0);	
        ObjectManager.instance.UnequipTool(this);
        trashManiaDisplay.Disable();
        trashManiaDisplay.gameObject.SetActive(false);
    }

    public void AddTime()
    {
        timer += timerLength;
    }

    public float GetTimer()
    {
        return timer;
    }
}
