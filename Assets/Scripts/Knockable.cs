using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Note: 
    >> Any GameObject attaching the Breakable component should also be assigned a ** Layer "Knockable" **.
	>> Any GameObject using the Knockable component should also have a ** RigidBody ** component.
	>> The RigidBody component must have useGravity checked.
*/
public class Knockable : MonoBehaviour
{
    [Header("Object Attributes")]
    // public float firstTimeScorePoint;
    // public float regularScorePoint;
    public string objName;
    public float scorePoint;
    public int level;                       // The floor this object is on
    public bool toppled;                    // Flag determining whether this object has been knocked over or not
    CentralHumanController CHC;             // Reference to the Central Human Controller

    private DestroyEffect df;
    private Rigidbody rb;
    private Collider cl;

    // The action type which is acted on this object
    private ScoreManager.ActionTypes aType = ScoreManager.ActionTypes.KNOCK;
    // The point at which force is applid
    private Vector3 collidePoint;
    //Audio Engine
    AudioSource KnockedSound;
    public AudioClip objectKnock;
    private bool _hasAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        // rb.centerOfMass = 0;
        collidePoint = transform.position; // + (rb.centerOfMass + Vector3.up * cl.bounds.size.y * 0.8f);
        toppled = false;

        CHC = GameObject.Find("CentralHumanController").GetComponent<CentralHumanController>();
        //CHC.registerObject(transform);

        KnockedSound = GetComponent<AudioSource>();
        StartCoroutine(AudioLoadIn());

        //Audio Engine
        if (_hasAudio)
        {
            KnockedSound.clip = objectKnock;
        }

        df = GetComponent<DestroyEffect>();
    }

    public void trigger(Vector3 pushForce)
    {
        pushForce.y = -Mathf.Abs(pushForce.x);
        rb.AddForceAtPosition(pushForce, collidePoint);

        //Debug.Log("collide at" + collidePoint);
        if (!toppled)
        {
            ScoreManager.instance.AddScore(objName, aType, scorePoint);
            toppled = true;
            TaskManager.instance.UpdateProgress(gameObject);
            df.StartDusting(false);
        }
    }

    public void OnCollisionStay(Collision col)
    {
        // Debug.Log("hit");
        if (_hasAudio)
        {
            float volume = Mathf.Clamp(col.relativeVelocity.magnitude / 38.0f, 0.0f, 1.0f);
            KnockedSound.PlayOneShot(objectKnock, volume);
            StartCoroutine(AudioWaitTime());
        }
    }

    private IEnumerator AudioWaitTime()
    {
        _hasAudio = false;
        yield return new WaitForSeconds(0.5F);
        _hasAudio = true;
    }

    private IEnumerator AudioLoadIn()
    {
        _hasAudio = false;
        yield return new WaitForSeconds(2);
        _hasAudio = true;
    }
}