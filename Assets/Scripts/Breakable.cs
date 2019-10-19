using UnityEngine;

/*
	Note: 
	Any GameObject using the Breakable component should also be assigned a Layer "Breakable".
	It is NOT using RigidBody for collision detection. 
*/
public class Breakable : MonoBehaviour
{
    public int total_health;
    public int health;


    private void Start()
    {
        health = total_health;
    }
    public void trigger(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        if (health <= 0)
        {
            health = 0;
            RaccoonController.score += total_health;
            Destroy(this.gameObject);
        }

    }

}
