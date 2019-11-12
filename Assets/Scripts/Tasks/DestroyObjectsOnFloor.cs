using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectsOnFloor : GameTask
{
    int numToDestroy;                       // Number of objects to destroy to complete this goal
    int level;                              // The relevant floor level
    bool active;                            // Flag determining whether this task is active or not
    List<Breakable> breakableObjects;       // List of breakable objects on the relevant floor level
    List<Breakable> destroyed;              // List of objects that have been destroyed so far

    // Intermediate variables
    Breakable[] B;

    override bool isComplete()
    {
        return destroyed.Count >= numToDestroy;
    }

    // Start is called before the first frame update
    void Start()
    {
        destroyed = new List<Breakable>();

        // Retrieve all breakable objects
        breakableObjects = new List<Breakable>();
        B = FindObjectsOfType<Breakable>();
        for (int i = 0; i < B.Length; i++)
        {
            if (B[i].level == level)
            {
                breakableObjects.Add(B[i]);
            }
        }
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
}
