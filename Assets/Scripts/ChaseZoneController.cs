using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseZoneController : MonoBehaviour
{
	public GameObject target;
	public GameObject owner;

	private Collider cl;
	private float bound_x1, bound_x2, bound_z1, bound_z2;
	private bool isChasing, seenTarget; 	// Boolean tags

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Collider from the GameObject
        cl = GetComponent<Collider>();

        //Fetch the size of the Collider volume
        Vector3 size = cl.bounds.size;
        bound_x1 = transform.position.x - size.x / 2;
        bound_x2 = transform.position.x + size.x / 2;
        bound_z1 = transform.position.z - size.z / 2;
        bound_z2 = transform.position.z + size.z / 2;

        isChasing = false;
        seenTarget = false;

    }

    // Update is called once per frame
    void Update()
    {
    	float m_x = target.transform.position.x;
    	float m_z = target.transform.position.z;
   		seenTarget = (m_x >= bound_x1 && m_x <= bound_x2 && m_z >= bound_z1 && m_z <= bound_z2) ? true : false;
       	if (seenTarget && !isChasing) {
    		FindObjectOfType<HumanController>().ChaseRaccoon();
	       	isChasing = true;
	    } else if (!seenTarget && isChasing) {
	       	FindObjectOfType<HumanController>().StopChaseRaccoon();
	       	isChasing = false;
       }

    }
}
