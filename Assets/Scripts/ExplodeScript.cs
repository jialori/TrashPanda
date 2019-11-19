using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    public bool startExplosion = false;
    public float destroyDelay = 5;


    // Update is called once per frame
    public void Explode(Vector3 momentum)
    {
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(momentum/rb.mass, ForceMode.Impulse);
            }
            Destroy(t.gameObject, destroyDelay);
        }

    }
}
