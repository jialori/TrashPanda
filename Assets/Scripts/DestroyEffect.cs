using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // Need to add DestroyEffect script to any breakable or knockable objects
    public float dustTime = 1;
    public GameObject effect;
    public float starLifetime = 3f; // 3 for Scaffolding, 1 for bucket


    private bool dusting = false;
    private float leftDustTime;
    private GameObject dust;

    void Start()
    {
        leftDustTime = dustTime;
    }


    void Update()
    {
        Dusting();
    }

    public void StartDusting(bool destroyed)
    {
        dusting = true;
        leftDustTime = dustTime;
        if (!dust) dust = Instantiate(effect, GetComponent<Collider>().bounds.center, Quaternion.identity);
        var psm = dust.GetComponent<ParticleSystem>().main;
        psm.startLifetime = starLifetime;
        dust.GetComponent<ParticleSystem>().Play();

        if (destroyed) Destroy(gameObject, (float)(dustTime * 0.9));
    }

    private void Dusting()
    {
        if (dusting) leftDustTime -= Time.deltaTime;

        if (leftDustTime <= 0)
        {
            dust.GetComponent<ParticleSystem>().Stop();
            dusting = false;
        }
    }
}
