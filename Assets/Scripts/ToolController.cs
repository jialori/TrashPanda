using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    public GameObject player;
    public RaccoonController playerScript;
    public float throwForce;
    public AudioClip sfx;
    public string toolType;

    bool hasPlayer = false;
    bool beingCarried = false;
    private Rigidbody rb;
    private static bool handFree = true;
    public static ToolController toolInHand = null;
    //private AudioSource audioSource;
    //private bool touched = false;
    // Start is called before the first frame update

    [Header("Tool Attributes")]
    public float effectOnAttack = 10;
    public float effectOnSpeed = 10;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerScript = player.GetComponent<RaccoonController>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (dis <= 3f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && handFree && Input.GetButtonDown("X"))//Input.GetKeyDown("i"))
        {
            rb.isKinematic = true;
            transform.parent = player.transform;
            beingCarried = true;
            transform.localPosition = player.transform.forward;
            handFree = false;
            toolInHand = this;
            Equip();

        }
        if (beingCarried)
        {
            //else if (touched)
            //{
            //rb.isKinematic = false;
            //transform.parent = null;
            //beingCarried = false;
            //touched = false;
            //}
            //if (Input.GetKeyDown("o"))
            //{
            //rb.isKinematic = false;
            //Vector3 currLoc = transform.position;
            //transform.parent = null;
            //transform.position = currLoc;
            //beingCarried = false;
            //rb.AddForce(player.forward * throwForce);
            //PlaySFX();
            //}
            if (Input.GetButtonDown("Y"))//Input.GetKeyDown("u"))
            {
                rb.isKinematic = false;
                Vector3 currLoc = transform.position;
                transform.parent = null;
                transform.position = currLoc;
                //transform.SetParent(null, false);
                beingCarried = false;
                handFree = true;
                toolInHand = null;
                UnEquip();
            }
        }
    }

    private void OnTriggerEnter()
    {
        if (beingCarried)
        {
            //touched = true;
        }
    }

    //void PlaySFX()
    //{
    //if (audioSource.isPlaying)
    //{
    //return;
    //}
    //audioSource.clip = sfx;
    //audioSource.Play();
    //}

    public void Equip() 
    {
    	playerScript.AddStrengthModifier(effectOnAttack, effectOnSpeed);
    }

    public void UnEquip()
    {
	    playerScript.RemoveStrengthModifier(effectOnAttack, effectOnSpeed);	
    }

}
