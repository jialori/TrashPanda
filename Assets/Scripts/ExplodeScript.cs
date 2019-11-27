using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    public bool startExplosion = false;
    public float destroyDelay = 5;

    public AudioSource SFX;
    public AudioClip BreakSFX;

    // Update is called once per frame
    public void Explode(Vector3 momentum)
    {
        GetComponent<AudioSource>();
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                SFX.PlayOneShot(BreakSFX, 0.5F);
                rb.AddForce(momentum/rb.mass, ForceMode.Impulse);
            }
            Destroy(t.gameObject, destroyDelay);
        }

    }
}
