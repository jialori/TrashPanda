using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CentralHumanController : MonoBehaviour
{
    public Transform target;                    // Human target to be chased (will always be the raccoon)
    public bool spotted = false;                // Flag determining if the raccoon has been spotted

    public List<Tuple<List<Breakable>, List<Knockable>>> allObjects;           // List of all breakable and knockable objects per floor
    public List<List<HumanController>> humans;                                 // List of humans currently in the game
    /*
    Tuple<List<Breakable>, List<Knockable>> objects_F1;
    Tuple<List<Breakable>, List<Knockable>> objects_F2;
    Tuple<List<Breakable>, List<Knockable>> objects_F3;
    Tuple<List<Breakable>, List<Knockable>> objects_F4;
    Tuple<List<Breakable>, List<Knockable>> objects_F5;
    */

    public void registerObject(Transform obj)
    {
        if (obj.TryGetComponent(out Breakable breakableObj))
        {
            allObjects[breakableObj.level - 1].Item1.Add(breakableObj);
        }
        else if (obj.TryGetComponent(out Knockable knockableObj))
        {
            allObjects[knockableObj.level - 1].Item2.Add(knockableObj);
        }
        else if (obj.TryGetComponent(out HumanController worker))
        {
            humans[worker.level - 1].Add(worker);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize variables
        humans = new List<List<HumanController>>();

        for (int i = 0; i < 5; i++)
        {
            humans[i] = new List<HumanController>();
        }
        /*
        H = GameObject.FindGameObjectsWithTag("Human");
        // Retrieve all enemies
        for (int i = 0; i < H.Length; i++)
        {
            h = H[i].GetComponent<HumanController>();
            humans.Add(h);
        }
        */

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
        // For each floor
        for (int i = 0; i < humans.Count; i++)
        {
            for(int j = 0; j < humans[i].Count; j++)
            // If the selected worker has seen the raccoon
            if (humans[i][j].seesRaccoon)
            {
                spotted = true;
            }
        }

        // If the raccoon has been spotted
        if (spotted)
        {
            //Debug.Log(1);
            // For each worker on the same floor as the raccoon
            for (int i = 0; i < humans[target.GetComponent<RaccoonController>().level].Count; i++)
            {
                //Debug.Log("2: " + i)
                // Inform worker of the location of the raccoon
                humans[target.GetComponent<RaccoonController>().level][i].lastKnownLocation = target.position;
                
            }
        }
    }
}
