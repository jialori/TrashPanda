using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public float radius;
    public bool startExplo = false;
    public float destroyDelay = 3;


    // Update is called once per frame
    public void Explode(Vector3 relativeForce)
    {
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(relativeForce, ForceMode.Impulse);
            }
            Destroy(t.gameObject, destroyDelay);
        }

    }
}
