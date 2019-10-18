using UnityEngine;

public class BinController : MonoBehaviour
{
    public GameObject player;
    bool isTouching;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E down");
            //Debug.Log(isTouching);
            if (isTouching)
                FindObjectOfType<HumanController>().ChaseRaccoon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("touch enter");
            isTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("touch leave");
            isTouching = false;
        }
    }


}
