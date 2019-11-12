using UnityEngine;

/*
	Note: 
	>> Any GameObject attaching the Breakable component should also be assigned a Layer "Breakable".
	>> It is NOT using RigidBody for collision detection. 
*/
public class Breakable : MonoBehaviour
{
    [Header("Object Attributes")]
    public float totalHealth;
    public float scorePoint;
    public float dustTime = 1;
    public GameObject DestroyEffect;

    private bool dusting = false;
    private float leftDustTime;
    private GameObject dust;
    
    private float health; 

    public float Health { get => health;}


    private void Start()
    {
        health = totalHealth;
        leftDustTime = dustTime;
    }


    private void Update()
    {
        Dusting();
    }

    private void StartDusting()
    {
        dusting = true;
        leftDustTime = dustTime;
        if (!dust) dust = Instantiate(DestroyEffect, GetComponent<Renderer>().bounds.center, Quaternion.identity);
        dust.GetComponent<ParticleSystem>().Play();
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
    public void trigger(float atk)
    {
        health -= calcDamage(atk);
        Debug.Log("Object health" + health);
        if (health <= 0)
        {
            health = 0;
            Debug.Log("broke some object");
            ScoreManager.instance.AddScore(scorePoint);
            StartDusting();
            Destroy(gameObject, (float)(dustTime * 0.9));
        }
        StartDusting();
    }

    public float calcDamage(float atk)
    {
        return atk;
    }
}
