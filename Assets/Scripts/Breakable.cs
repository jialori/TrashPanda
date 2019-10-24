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
    
    private float health;


    private void Start()
    {
        health = totalHealth;
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
            Destroy(this.gameObject);
        }

    }

    public float calcDamage(float atk)
    {
        return atk;
    }

}
