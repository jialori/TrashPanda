using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CentralHumanController : MonoBehaviour
{
    public Transform target;                    // Human target to be chased (will always be the raccoon)
    public bool spotted = false;                // Flag determining if the raccoon has been spotted

    public List<Tuple<List<Breakable>, List<Knockable>>> allObjects;            // List of all breakable and knockable objects per floor
    List<HumanController> humans;                                               // List of humans currently in the game
    /*
    Tuple<List<Breakable>, List<Knockable>> objects_F1;
    Tuple<List<Breakable>, List<Knockable>> objects_F2;
    Tuple<List<Breakable>, List<Knockable>> objects_F3;
    Tuple<List<Breakable>, List<Knockable>> objects_F4;
    Tuple<List<Breakable>, List<Knockable>> objects_F5;
    */

    // Intermediate variables
    GameObject[] H;
    HumanController h;

    public void registerObject(Transform obj)
    {
        if (obj.TryGetComponent(out Breakable breakableObj))
        {
            allObjects[breakableObj.level - 1].Item1.Add(breakableObj);
            Debug.Log("CHC: Added " + breakableObj.ToString() + " to breakable objects on level " + breakableObj.level.ToString());
            Debug.Log("CHC: Now has " + allObjects[breakableObj.level - 1].Item1.Count + " breakable objects on floor " + breakableObj.level.ToString());
        }
        else if (obj.TryGetComponent(out Knockable knockableObj))
        {
            allObjects[knockableObj.level - 1].Item2.Add(knockableObj);
            Debug.Log("CHC: Added " + knockableObj.ToString() + " to knockable objects on level " + knockableObj.level.ToString());
            Debug.Log("CHC: Now has " + allObjects[knockableObj.level - 1].Item1.Count + " knockable objects on floor " + knockableObj.level.ToString());
        }
        /*
        else if (obj.TryGetComponent(out HumanController worker))
        {
            humans[worker.level - 1].Add(worker);
        }
        */
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize variables
        humans = new List<HumanController>();
        
        H = GameObject.FindGameObjectsWithTag("Human");
        // Retrieve all enemies
        for (int i = 0; i < H.Length; i++)
        {
            h = H[i].GetComponent<HumanController>();
            humans.Add(h);
        }
        

        allObjects = new List<Tuple<List<Breakable>, List<Knockable>>>();

        for (int i = 0; i < 5; i++)
        {
            allObjects[i] = new Tuple<List<Breakable>, List<Knockable>>(new List<Breakable>(), new List<Knockable>());
        }
        /*
        objects_F1 = new Tuple<List<Breakable>, List<Knockable>>();
        objects_F1[0] = new List<Breakable>();
        objects_F1[1] = new List<Knockable>();

        objects_F2 = new Tuple<List<Breakable>, List<Knockable>>();
        objects_F2[0] = new List<Breakable>();
        objects_F2[1] = new List<Knockable>();

        objects_F3 = new Tuple<List<Breakable>, List<Knockable>>();
        objects_F3[0] = new List<Breakable>();
        objects_F3[1] = new List<Knockable>();

        objects_F4 = new Tuple<List<Breakable>, List<Knockable>>();
        objects_F4[0] = new List<Breakable>();
        objects_F4[1] = new List<Knockable>();

        objects_F5 = new Tuple<List<Breakable>, List<Knockable>>();
        objects_F5[0] = new List<Breakable>();
        objects_F5[1] = new List<Knockable>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any of the humans have spotted the raccoon
        spotted = false;
        // For each worker
        for (int i = 0; i < humans.Count; i++)
        {
            // If the selected worker has seen the raccoon
            if (humans[i].seesRaccoon)
            {
                spotted = true;
            }
        }

        // If the raccoon has been spotted
        if (spotted)
        {
            //Debug.Log(1);
            // For each worker
            for (int i = 0; i < humans.Count; i++)
            {
                //Debug.Log("2: " + i);
                // If the selected worker is on the same floor as the raccoon
                if (humans[i].onSameFloor(target))
                {
                    // Inform worker of the location of the raccoon
                    humans[i].lastKnownLocation = target.position;
                }
            }
        }
    }
}
