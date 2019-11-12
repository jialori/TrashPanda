using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float dustTime = 1;
    public GameObject effect;


    private bool dusting = false;
    private float leftDustTime;
    private GameObject dust;
    // Start is called before the first frame update
    void Start()
    {
        leftDustTime = dustTime;
    }

    // Update is called once per frame
    void Update()
    {
        Dusting();
    }

    public void StartDusting(bool destroyed)
    {
        dusting = true;
        leftDustTime = dustTime;
        if (!dust) dust = Instantiate(effect, GetComponent<Renderer>().bounds.center, Quaternion.identity);
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
