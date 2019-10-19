using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    /*
	public float speed;

	private bool isChasing;
    private Rigidbody rb;
	private GameObject raccoon;
    private Vector3 originalPosition;
    private Vector3 direction;
    */

    public Transform target;
    public Vector3 destination;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        /*
        rb = GetComponent<Rigidbody>();
        raccoon = GameObject.FindWithTag("Raccoon");
        originalPosition = transform.position;
        isChasing = false;
        */
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update() {

        /*
    	if (isChasing) {
			// direction = (raccoon.transform.position - transform.position).normalized;
    		// rb.velocity = direction * speed * Time.deltaTime;
    		// Debug.Log("is Chasing!");
    		transform.position = Vector3.MoveTowards(transform.position, raccoon.transform.position, speed * Time.deltaTime);
    	} else if (!isChasing && originalPosition != transform.position) {
			// direction = (originalPosition - transform.position).normalized;
    		// rb.velocity = direction * speed * Time.deltaTime;
    		transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
    	}
        */
        agent.SetDestination(destination);
    }

    /*
    public void ChaseRaccoon() {
    	isChasing = true;
    }

    public void StopChaseRaccoon() {
    	isChasing = false;
    }

    public bool IsChasing() {
        return isChasing;
    }
    */
}
