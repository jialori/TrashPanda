using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    public Transform player;
    public float throwForce;
    public AudioClip sfx;
    public string toolType;
    
    
    public int dmg;

    bool hasPlayer = false;
    public bool beingCarried = false;
    private Rigidbody rb;
    //private AudioSource audioSource;
    //private bool touched = false;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (dis <= 5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && Input.GetKeyDown("i"))
        {
            rb.isKinematic = true;
            transform.parent = player;
            beingCarried = true;
            transform.localPosition = player.forward;
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
            if (Input.GetKeyDown("u"))
            {
                rb.isKinematic = false;
                Vector3 currLoc = transform.position;
                transform.parent = null;
                transform.position = currLoc;
                //transform.SetParent(null, false);
                beingCarried = false;
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

}
