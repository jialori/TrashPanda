using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
	public float speed;

	private bool isChasing;
    private Rigidbody rb;
	private GameObject raccoon;
    private Vector3 originalPosition;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        raccoon = GameObject.FindWithTag("Raccoon");
        originalPosition = transform.position;
        isChasing = false;
    }

    void Update() {

    	if (isChasing) {
			direction = (raccoon.transform.position - transform.position).normalized;
    		rb.velocity = direction * speed * Time.deltaTime;
    		Debug.Log("is Chasing!");
    	} else if (!isChasing && originalPosition != transform.position) {
			direction = (originalPosition - transform.position).normalized;
    		rb.velocity = direction * speed * Time.deltaTime;
    	}

    }

    public void ChaseRaccoon() {
    	isChasing = true;
    }

    public void StopChaseRaccoon() {
    	isChasing = false;
    }
}
