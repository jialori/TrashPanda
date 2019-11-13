using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CentralHumanController : MonoBehaviour
{
    public bool verboseMode;
    public Transform target;                    // Human target to be chased (will always be the raccoon)
    public bool spotted = false;                // Flag determining if the raccoon has been spotted

    public List<Tuple<List<Breakable>, List<Knockable>>> allObjects;            // List of all breakable and knockable objects per floor
    List<HumanController> humans;                                               // List of humans currently in the game

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
        var humanObjs = GameObject.FindGameObjectsWithTag("Human");
        // Debug.Log("humanObjs length: " + humanObjs.Length);
        humans = humanObjs.Select(obj => obj.GetComponent<HumanController>()).ToList();
        // Debug.Log("human: " + humans);


        allObjects = new List<Tuple<List<Breakable>, List<Knockable>>>();

        for (int i = 0; i < 5; i++)
        {
            allObjects.Add(new Tuple<List<Breakable>, List<Knockable>>(new List<Breakable>(), new List<Knockable>()));
        }

        // Level 1 objects
        var B1 = GameObject.Find("Level 1 Breakables").GetComponentsInChildren<Breakable>();
        var K1 = GameObject.Find("Level 1 Knockables").GetComponentsInChildren<Knockable>();
        if (verboseMode) Debug.Log("K1 length: " + K1.Length.ToString());
        if (verboseMode) Debug.Log("B1 length: " + B1.Length.ToString());
        foreach (var obj in K1)
        {
            allObjects[0].Item2.Add(obj);
        }
        foreach (var obj in B1)
        {
            allObjects[0].Item1.Add(obj);
        }
        if (verboseMode) Debug.Log("Initialized B1 length: " + allObjects[0].Item1.Count());
        if (verboseMode) Debug.Log("Initialized K1 length: " + allObjects[0].Item2.Count());

        // Level 2 objects
        var B2 = GameObject.Find("Level 2 Breakables").GetComponentsInChildren<Breakable>();
        var K2 = GameObject.Find("Level 2 Knockables").GetComponentsInChildren<Knockable>();
        if (verboseMode) Debug.Log("K2 length: " + K2.Length.ToString());
        if (verboseMode) Debug.Log("B2 length: " + B2.Length.ToString());
        foreach (var obj in B2)
        {
            allObjects[1].Item1.Add(obj);
        }
        foreach (var obj in K2)
        {
            allObjects[1].Item2.Add(obj);
        }
        if (verboseMode) Debug.Log("Initialized B2 length: " + allObjects[1].Item1.Count());
        if (verboseMode) Debug.Log("Initialized K2 length: " + allObjects[1].Item2.Count());


        // Level 3 Objects
        var B3 = GameObject.Find("Level 3 Breakables").GetComponentsInChildren<Breakable>();
        var K3 = GameObject.Find("Level 3 Knockables").GetComponentsInChildren<Knockable>();
        if (verboseMode) Debug.Log("K3 length: " + K3.Length.ToString());
        if (verboseMode) Debug.Log("B3 length: " + B3.Length.ToString());
        foreach (var obj in B3)
        {
            allObjects[2].Item1.Add(obj);
        }
        foreach (var obj in K3)
        {
            allObjects[2].Item2.Add(obj);
        }
        if (verboseMode) Debug.Log("Initialized B3 length: " + allObjects[2].Item1.Count());
        if (verboseMode) Debug.Log("Initialized K3 length: " + allObjects[2].Item2.Count());


        // Level 4 Objects
        var B4 = GameObject.Find("Level 4 Breakables").GetComponentsInChildren<Breakable>();
        var K4 = GameObject.Find("Level 4 Knockables").GetComponentsInChildren<Knockable>();
        if (verboseMode) Debug.Log("K4 length: " + K4.Length.ToString());
        if (verboseMode) Debug.Log("B4 length: " + B4.Length.ToString());
        foreach (var obj in B4)
        {
            allObjects[3].Item1.Add(obj);
        }
        foreach (var obj in K4)
        {
            allObjects[3].Item2.Add(obj);
        }
        if (verboseMode) Debug.Log("Initialized B4 length: " + allObjects[3].Item1.Count());
        if (verboseMode) Debug.Log("Initialized K4 length: " + allObjects[3].Item2.Count());

        // Level 4 Objects
        var B5 = GameObject.Find("Level 5 Breakables").GetComponentsInChildren<Breakable>();
        var K5 = GameObject.Find("Level 5 Knockables").GetComponentsInChildren<Knockable>();
        if (verboseMode) Debug.Log("K5 length: " + K5.Length.ToString());
        if (verboseMode) Debug.Log("B5 length: " + B5.Length.ToString());
        foreach (var obj in B5)
        {
            allObjects[4].Item1.Add(obj);
        }
        foreach (var obj in K5)
        {
            allObjects[4].Item2.Add(obj);
        }
        if (verboseMode) Debug.Log("Initialized B5 length: " + allObjects[4].Item1.Count());
        if (verboseMode) Debug.Log("Initialized K5 length: " + allObjects[4].Item2.Count());
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
