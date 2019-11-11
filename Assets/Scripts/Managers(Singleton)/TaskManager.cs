using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    /* 
     * Create a list of tasks could be completed to gain tools for increased damage. 
     * Tools will expire after a certain time limit. Examples of tasks include:
     * - Destroy x objects on floor y
     * - Destroy the [unique object]
     * 
     * A task is represented by a boolean condition and a description. The task is 
     * considered completed when the boolean condition returns true
    */
    List<Tuple<bool, string>> activeTasks;                  // The tasks that are active and can be completed
    List<Tuple<bool, string>> taskPool;                     // The tasks that can be added to 'activeTasks'
    List<Tool> tools;                                       // A list of tools that could be generated as a result of completing tasks
    Random rnd;                                             // Random number generator

    List<Breakable> breakableObjects;                       // List of breakable objects
    List<Knockable> knockableObjects;                       // List of knockable objects

    // Intermediate variables
    Breakable[] B;
    Knockable[] K;

    // Makes a task from 'taskPool' active so that it can be completed by the player
    void addRandomTask(List<Tuple<bool, string>> activeTasks, List<Tuple<bool, string>> taskPool, Random rnd)
    {
        int i = rnd.Next(taskPool.Count);
        activeTasks.Add(taskPool[i]);
        // Remove the added task from taskPool so that it can't be added again
        taskPool.RemoveAt(i);
    }

    void Start()
    {
        // Instantiate lists
        activeTasks = new List<Tuple<bool, string>>();
        taskPool = new List<Tuple<bool, string>>();

        // Retrieve all breakable objects
        B = FindObjectsOfType<Breakable>();
        breakableObjects = B.OfType<Breakable>().ToList();

        // Retrieve all knockable objects
        K = FindObjectsOfType<Knockable>();
        knockableObjects = K.OfType<Knockable>().ToList();

        // Initialize random number generator
        rnd = new Random();

        // Add possible tasks here

        
    }
    
    void Update()
    {
        // For each active task
        for (int i = 0; i < activeTasks.Count; i++)
        {
            // If the task has been completed
            if (activeTasks[i][0])
            {
                // Remove the completed task and add a new one
                activeTasks.RemoveAt(i);
                addRandomTask(activeTasks, taskPool, rnd);

                // Generate a tool for the raccoon here

                // Continuing to iterate over a list after modifying it is risky. So we'll break here
                break;
            }
        }
    }
}
