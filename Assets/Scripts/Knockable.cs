using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Note: 
  >> Any GameObject attaching the Breakable component should also be assigned a ** Layer "Knockable" **.
	>> Any GameObject using the Knockable component should also have a ** RigidBody ** component.
	>>   The RigidBody component must have useGravity checked.
*/
public class Knockable : MonoBehaviour
{
    [Header("Object Attributes")]
    // public float firstTimeScorePoint;
    // public float regularScorePoint;
    public float scorePoint;

    private bool pointCollected;


    private Rigidbody rb;
    private Collider cl;
    // The point at which force is applid
    private Vector3 collidePoint;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        // rb.centerOfMass = 0;
        collidePoint = transform.position;// + (rb.centerOfMass + Vector3.up * cl.bounds.size.y * 0.8f);
        pointCollected = false;
    }


  	public void trigger(Vector3 pushForce) 
  	{
  		pushForce.y = - Mathf.Abs(pushForce.x);
  		rb.AddForceAtPosition(pushForce, collidePoint);

      Debug.Log("collide at" + collidePoint);
      if (!pointCollected) {
        ScoreManager.instance.AddScore(scorePoint);
        pointCollected = true;
      }
  	}


  //   void Update() {
  //       rotateTarget = transform.forward;
  //   	Vector3 dir = Vector3.RotateTowards(transform.forward, rotateTarget, rotateSpeed * Time.deltaTime, 0.0f);
		// transform.rotation = Quaternion.LookRotation(dir);
  //       transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
  //   }

}
