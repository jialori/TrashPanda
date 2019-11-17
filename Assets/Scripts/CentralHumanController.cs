using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CentralHumanController : MonoBehaviour
{
    public bool verboseMode;
    public Transform target;                    // Human target to be chased (will always be the raccoon)
    public Transform LKL;
    public bool spotted = false;                // Flag determining if the raccoon has been spotted

    public List<Tuple<List<Breakable>, List<Knockable>>> allObjects;            // List of all breakable and knockable objects per floor
    List<HumanController> humans;                                               // List of humans currently in the game

    public void registerObject(GameObject obj)
    {
        if (obj.TryGetComponent(out Breakable breakableObj))
        {
            allObjects[breakableObj.level - 1].Item1.Add(breakableObj);
            // Debug.Log("CHC: Added " + breakableObj.ToString() + " to breakable objects on level " + breakableObj.level.ToString());
            // Debug.Log("CHC: Now has " + allObjects[breakableObj.level - 1].Item1.Count + " breakable objects on floor " + breakableObj.level.ToString());
        }
        else if (obj.TryGetComponent(out Knockable knockableObj))
        {
            allObjects[knockableObj.level - 1].Item2.Add(knockableObj);
            // Debug.Log("CHC: Added " + knockableObj.ToString() + " to knockable objects on level " + knockableObj.level.ToString());
            // Debug.Log("CHC: Now has " + allObjects[knockableObj.level - 1].Item1.Count + " knockable objects on floor " + knockableObj.level.ToString());
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
        // LKL = GameObject.Find("LastKnownLocation").transform;
        // var humanObjs = GameObject.FindGameObjectsWithTag("Human");
        // Debug.Log("humanObjs length: " + humanObjs.Length);
        // humans = humanObjs.Select(obj => obj.GetComponent<HumanController>()).ToList();
        // Debug.Log("human: " + humans);

        allObjects = new List<Tuple<List<Breakable>, List<Knockable>>>(5);

        for (int i = 0; i < 5; i++)
        {
            allObjects.Add(new Tuple<List<Breakable>, List<Knockable>>(new List<Breakable>(), new List<Knockable>()));
        }
    }

    void Start()
    {
        humans = GameManager.instance.Workers;
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
            LKL.position = target.position;
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
