using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveToolController : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float range;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private GameObject spawn; 
    
    // variables to control frequency of use
    private float cooldown = 10f;
    private bool canUse = true;
    private float cooldownTimer = 0; 
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            

            if (spawn)
            {
                spawn.GetComponent<CharacterController>().Move(spawn.transform.forward);

                if (Mathf.Abs(Vector3.Distance(spawn.transform.position, originalPosition)) > range) 
                {
                    Destroy(spawn);
                }
            }
        }
        else {
            if (spawn) Destroy(spawn);
            canUse = true;
        }
    }

    // Interact with this tool
    public void Activate()
    {
        if (canUse)
        {
            // set cooldown timer
            canUse = false;
            cooldownTimer = cooldown;
        
            // Create clone to shoot out
            spawn = Instantiate(this.gameObject, transform.position + transform.forward * 1.5f, transform.rotation);
            
            // Use Rigidbody
            // spawn.GetComponent<Rigidbody>().isKinematic = false;
            // spawn.GetComponent<Rigidbody>().AddForce(thrust * spawn.transform.forward * 5000);
        }
        else
        {
            Debug.Log("On Cooldown");
        }
    }

    // On collision, knock Knockable objects over
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Knock over a knock-over-able objects
        Knockable knockable = hit.gameObject.GetComponent("Knockable") as Knockable;
        if (knockable != null)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3 pushForce = pushDir * thrust;
            knockable.trigger(pushForce);
        }
    }

}
