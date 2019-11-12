using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    List<GameTask> activeTasks; // The tasks that are active and can be completed
    List<GameTask> taskPool; // The tasks that can be added to 'activeTasks'
    public List<GameObject> tools; // A list of tools that could be generated as a result of completing tasks
    [SerializeField] private GameObject objectiveComplete;
    [SerializeField] private TextMeshProUGUI description;
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
            DontDestroyOnLoad(gameObject);
        }
    }

    // Makes a task from 'taskPool' active so that it can be completed by the player
    void addRandomTask()
    {
        int i = Random.Range(0, taskPool.Count);
        if (activeTasks.Find(task => task == taskPool[i]) == null)
            activeTasks.Add(taskPool[i]);
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
        var completedTaskIdx = -1;
        // For each active task
        for (int i = 0; i < activeTasks.Count; i++)
        {
            // If the task has been completed
            if (activeTasks[i].isComplete())
            {
                Debug.Log("Task " + activeTasks[i].description + " has been completed");
                completedTaskIdx = i;
                break;
            }
        }
        // Completed a task
        if (completedTaskIdx != -1)
        {
            // Remove the completed task and add a new one
            var completedTask = activeTasks[completedTaskIdx];
            activeTasks.RemoveAt(completedTaskIdx);
            if (taskPool.Count > 0)
            {
                addRandomTask();
            }

            // Show objective complete
            ShowObjectiveComplete(completedTask);

            // Generate a tool for the raccoon here
            SpawnRandomTool();
        }
    }

    private void SpawnRandomTool()
    {
        if (ObjectManager.curTool == null)
        {
            var randTool = tools[Random.Range(0, tools.Count - 1)];
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
            Debug.Log("Knockable Progress");
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
            Debug.Log("Update progress for task");
            task.updateProgress(progress);
        }
    }

    private IEnumerator ShowObjectiveComplete(GameTask completedTask)
    {
        description.text = completedTask.description;
        objectiveComplete.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        objectiveComplete.SetActive(false);
    }
}