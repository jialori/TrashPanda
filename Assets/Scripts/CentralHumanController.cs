using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralHumanController : MonoBehaviour
{
    public Transform target;                // Human target to be chased (will always be the raccoon)
    List<HumanController> chasingHumans;    // Humans that are chasing the raccoon (and have not been assigned a direction)
    GameObject[] H;
    HumanController h;
    //List<HumanController> directions;                   // Outlines which directions the humans are trying to trap the raccoon from (will only be at most four elements long)

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        chasingHumans = new List<HumanController>();
        H = GameObject.FindGameObjectsWithTag("Human");
        //directions = new List<HumanController>();
        // Retrieve all enemies
        for (int i = 0; i < H.Length; i++)
        {
            h = H[i].GetComponent<HumanController>();
            chasingHumans.Add(h);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chasingHumans.Count >= 4)
        {
            chasingHumans[0].destination = target.position + Vector3.forward;
            chasingHumans[1].destination = target.position + Vector3.left;
            chasingHumans[2].destination = target.position + Vector3.right;
            chasingHumans[3].destination = target.position + Vector3.back;
        }
    }
}
