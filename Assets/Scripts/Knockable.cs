using UnityEngine;

/*
	Note: 
	Any GameObject using the Knockable component should also have a RigidBody component.
	AND the RigidBody component must have useGravity checked.
*/
public class Knockable : MonoBehaviour
{
    // [SerializeField] private float torquePower = 20;
    private Rigidbody rb;
    private Collider cl;
    private Vector3 collidePoint;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        // rb.centerOfMass = 0;
        collidePoint = transform.position;// + (rb.centerOfMass + Vector3.up * cl.bounds.size.y * 0.8f);

    }


    public void trigger(Vector3 pushForce)
    {
        // Debug.Log("collide at" + collidePoint);
        pushForce.y = -Mathf.Abs(pushForce.x);
        rb.AddForceAtPosition(pushForce, collidePoint);
    }


    //   void Update() {
    //       rotateTarget = transform.forward;
    //   	Vector3 dir = Vector3.RotateTowards(transform.forward, rotateTarget, rotateSpeed * Time.deltaTime, 0.0f);
    // transform.rotation = Quaternion.LookRotation(dir);
    //       transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
    //   }

}
