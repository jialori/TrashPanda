using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectsOnFloorTask : GameTask
{
    int numToDestroy;                       // Number of objects to destroy to complete this goal
    int level;                              // The relevant floor level
    bool active;                            // Flag determining whether this task is active or not
    List<Breakable> breakableObjects;       // List of breakable objects on the relevant floor level
    List<Breakable> destroyed;              // List of objects that have been destroyed so far

    CentralHumanController CHC;

    // Intermediate variables
    Breakable[] B;

    // Start is called before the first frame update
    void Start()
    {
        description = "Smash " + numToDestroy + " items on floor " + level;

        destroyed = new List<Breakable>();

        CHC = GameObject.Find("CentralHumanController").GetComponent<CentralHumanController>();

        // Retrieve all breakable objects
        breakableObjects = CHC.allObjects[level - 1].Item1;

    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            for (int i = 0; i < breakableObjects.Count; i++)
            {
                if (breakableObjects[i].destroyed && !destroyed.Contains(breakableObjects[i]))
                {
                    destroyed.Add(breakableObjects[i]);
                }
            }
        }
    }

    public override bool isComplete()
    {
        return destroyed.Count >= numToDestroy;
    }

    public override void updateProgress(TaskProgress progress)
    {

    }

    public override void Reset()
    {
        destroyed = new List<Breakable>();
    }

}
