using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Note: 
	Any GameObject using the Knockable component should also have a RigidBody component.
*/
public class Knockable : MonoBehaviour
{
    [SerializeField] private float torquePower = 20;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	public void trigger(Vector3 pushForce) 
	{
		Vector3 torqueForce = Vector3.up * torquePower;
		pushForce.y += 20;
		rb.AddForce(pushForce);
		rb.AddTorque(torqueForce);

		// Future consideration: maybe account the weight of object
	}

}
