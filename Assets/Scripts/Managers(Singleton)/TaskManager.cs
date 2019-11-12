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
    List<GameTask> activeTasks;                 // The tasks that are active and can be completed
    List<GameTask> taskPool;                    // The tasks that can be added to 'activeTasks'
    public List<GameObject> tools;                       // A list of tools that could be generated as a result of completing tasks

    public static TaskManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Makes a task from 'taskPool' active so that it can be completed by the player
    void addRandomTask()
    {
        int i = Random.Range(0, taskPool.Count);
        activeTasks.Add(taskPool[i]);
        // Remove the added task from taskPool so that it can't be added again
        taskPool.RemoveAt(i);
    }

    void Start()
    {
        // Instantiate lists
        activeTasks = new List<GameTask>();
        taskPool = new List<GameTask>();

        // Add possible tasks here
        taskPool.Add(new KnockOverNItemsTask(30));
        taskPool.Add(new KnockOverNItemsTask(10));
        taskPool.Add(new KnockOverNItemsTask(20));
        taskPool.Add(new KnockOverNSpecificItemsTask(3, "Ladder"));
        taskPool.Add(new KnockOverNSpecificItemsTask(5, "Ladder"));
        taskPool.Add(new KnockOverNSpecificItemsTask(10, "Brick"));
        taskPool.Add(new KnockOverNSpecificItemsTask(15, "Brick"));
        taskPool.Add(new KnockOverNSpecificItemsTask(3, "Wheelbarrow"));
        taskPool.Add(new KnockOverNSpecificItemsTask(5, "Wheelbarrow"));
        taskPool.Add(new KnockOverNSpecificItemsTask(3, "Paint Bucket"));
        taskPool.Add(new KnockOverNSpecificItemsTask(5, "Tool Box"));
        
        addRandomTask();
        addRandomTask();
        addRandomTask();

        for (int i = 0; i < activeTasks.Count; i++)
            Debug.Log(activeTasks[i].description);
    }
    
    void Update()
    {
        var completedTask = -1;
        // For each active task
        for (int i = 0; i < activeTasks.Count; i++)
        {
            // If the task has been completed
            if (activeTasks[i].isComplete())
            {
                Debug.Log("Task " + activeTasks[i].description + " has been completed");
                completedTask = i;
                break;
            }
        }

        if (completedTask != -1)
        {
            // Remove the completed task and add a new one
            activeTasks.RemoveAt(completedTask);
            if (taskPool.Count > 0)
            {
                addRandomTask();
            }

            // Generate a tool for the raccoon here
            SpawnRandomTool();
        }
    }

    private void SpawnRandomTool()
    {
        if (ObjectManager.curTool == null) 
        {
            var randTool = tools[Random.Range(0, tools.Count)];
            var randToolItem = Instantiate(randTool, new Vector3(0, 0, 0), Quaternion.identity);
            randToolItem.GetComponent<Tool>()?.Equip();
        }
        else 
        {
            ObjectManager.curTool.AddTime();
        }
    }

    public void UpdateProgress(GameObject obj)
    {
        if (obj.GetComponent<Knockable>() != null)
        {
            UpdateProgressForTasks(new TaskProgress(TaskProgress.TaskType.Knockable, obj));
        }
        else if (obj.GetComponent<Breakable>() != null)
        {
            UpdateProgressForTasks(new TaskProgress(TaskProgress.TaskType.Breakable, obj));
        }
    }

    private void UpdateProgressForTasks(TaskProgress progress)
    {
        foreach (var task in activeTasks)
        {
            task.updateProgress(progress);
        }
    }
}
