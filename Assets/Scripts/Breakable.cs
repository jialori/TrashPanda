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
    public int level;               // The floor this object is on
    public bool destroyed;          // Flag determining whether this object has been destroyed or not
    AudioSource breakSound;         // Audiosource files and script
    public AudioClip breakSFX;      //SFX for breaking object
    public AudioClip hitSFX;        //SFX for hitting object

    private float health; 

    public float Health { get => health;}

    private ScoreManager.ActionTypes aType = ScoreManager.ActionTypes.BREAK;
    public string name;

    private void Start()
    {
        health = totalHealth;
        destroyed = false;

        breakSound = GetComponent<AudioSource>();
    }

    public void trigger(float atk)
    {
        health -= calcDamage(atk);
        Debug.Log("Object health" + health);

        breakSound.PlayOneShot(hitSFX, 0.8F);

        if (health <= 0)
        {
            health = 0;
            //Debug.Log("broke some object");
            ScoreManager.instance.AddScore(name, aType, scorePoint);
            Destroy(this.gameObject);
            destroyed = true;
            //Debug.Log("destroyed: " + destroyed.ToString() + ", position: " + transform.position.ToString());
        }
    }

    public float calcDamage(float atk)
    {
        return atk;
    }
}
