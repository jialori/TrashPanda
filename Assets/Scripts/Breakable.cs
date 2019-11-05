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
    public int level;
    
    private float health; 

    public float Health { get => health;}


    private void Start()
    {
        health = totalHealth;
        if (transform.position.y < 7)
        {
            level = 1;
        }
        else if (7 <= transform.position.y && transform.position.y < 14)
        {
            level = 2;
        }
        else if (14 <= transform.position.y && transform.position.y < 21)
        {
            level = 3;
        }
        else if (21 <= transform.position.y && transform.position.y < 28)
        {
            level = 4;
        }
        else
        {
            level = 5;
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
            Destroy(this.gameObject);
        }

    }

    public float calcDamage(float atk)
    {
        return atk;
    }
}
