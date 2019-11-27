using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    List<GameTask> completedTasks; // The tasks that have been completed
    public List<GameObject> tools; // A list of tools that could be generated as a result of completing tasks
    [SerializeField] public GameObject objectiveComplete;
    [SerializeField] public TextMeshProUGUI description;
    [SerializeField] public GameObject newObjective;
    [SerializeField] public TextMeshProUGUI newDescription;
    [SerializeField] public TrashManiaDisplay trashManiaDisplay;
    [HideInInspector] public List<TextMeshProUGUI> pauseMenuTasks;
    [HideInInspector] public List<TextMeshProUGUI> countdownTasks;
    [HideInInspector] public List<TextMeshProUGUI> onScreenTasks;
    private bool linkedUI = false;
    private bool filledUI = false;

    public AudioSource SFX;
    public AudioClip ObjCompleteSFX;

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

        // Instantiate lists
        activeTasks = new List<GameTask>() { null, null, null };
        taskPool = new List<GameTask>();
        completedTasks = new List<GameTask>();
    }

    void Start()
    {
        // // Add possible tasks here
        InitializeTasks();

        while (TaskManager.instance.activeTasks.Any(task => task == null)){
            addRandomTask();
            Debug.Log("alooo");
        }

        TaskManager.instance.StartCoroutine("AddActiveTasksToUI");

        GetComponent<AudioSource>();

    }

    void Update()
    {
        if (!SceneTransitionManager.instance.isGameOn()) return;
        // Debug.Log("hi~~~~~");
        if (!TaskManager.instance.filledUI) return; // used once since GAME scene is never destroyed
        // Debug.Log("heyyy");
        // Debug.Log(addTextDoneCountdown);

        var completedTaskIdx = -1;
        // For each active task
        for (int i = 0; i < activeTasks.Count; i++)
        {
            // If the task has been completed
            if (TaskManager.instance.activeTasks[i].isComplete())
            {
                Debug.Log("Task " + TaskManager.instance.activeTasks[i].description + " has been completed");
                completedTaskIdx = i;
                break;
            }

            pauseMenuTasks[i].text = TaskManager.instance.activeTasks[i].description;
            onScreenTasks[i].text = TaskManager.instance.activeTasks[i].description;
        }
        // Completed a task
        if (completedTaskIdx != -1)
        {
            // Remove the completed task and add a new one
            SFX.PlayOneShot(ObjCompleteSFX, 0.6F);
            var completedTask = activeTasks[completedTaskIdx];
            activeTasks[completedTaskIdx] = null;
            completedTasks.Add(completedTask);
            GameTask newTask = null;
            newTask = addRandomTask();
            Debug.Log(newTask);
            pauseMenuTasks[completedTaskIdx].text = newTask.description;
            onScreenTasks[completedTaskIdx].text = newTask.description;

            // Show objective complete
            // Debug.Log("Calling showobjectivecomplete");
            TaskManager.instance.StopCoroutine("ShowObjectiveComplete");
            TaskManager.instance.StopCoroutine("ShowNewObjective");
            TaskManager.instance.StartCoroutine(ShowObjectiveComplete(completedTask));
            TaskManager.instance.StartCoroutine(ShowNewObjective(newTask));

            // Generate a tool for the raccoon here
            // Debug.Log("Calling spawnrandomtool");

            SpawnRandomTool();
        }

        // update task text if there are changes
    }

    // Makes a task from 'taskPool' active so that it can be completed by the player
    GameTask addRandomTask()
    {
        // Find the slot to add new task to
        int idxToAdd = -1;
        for (int i = 0; i < TaskManager.instance.activeTasks.Count; i++)
        {
            if (TaskManager.instance.activeTasks[i] == null)
            {
                idxToAdd = i;
            }
        }

        if (idxToAdd == -1) return null;

        // Find a new task to add
        var choices = Enumerable.Range(0, taskPool.Count - 1).ToList();
        var randomIdx = Random.Range(0, choices.Count());
        int newTaskIdx = choices[randomIdx];
        var newTask = taskPool[newTaskIdx];

        // Check to make sure the new task isn't a repeat
        bool hasKnockOverNItemsTask = false;
        var specificItems = new List<string>();
        foreach (var task in TaskManager.instance.activeTasks)
        {
            if (task as KnockOverNItemsTask != null)
            {
                hasKnockOverNItemsTask = true;
                continue;
            }
            var specificTask = task as KnockOverNSpecificItemsTask;
            if (specificTask != null)
            {
                specificItems.Add(specificTask.getItemType());
            }
        }

        while (true)
        {
            if (newTask.GetType() == typeof(KnockOverNItemsTask))
            {
                if (!hasKnockOverNItemsTask)
                {
                    break;
                }
            }
            var newSpecificTask = newTask as KnockOverNSpecificItemsTask;
            if (newSpecificTask != null)
            {
                if (!specificItems.Contains(newSpecificTask.getItemType()))
                {
                    break;
                }
            }

            choices.Remove(newTaskIdx);
            randomIdx = Random.Range(0, choices.Count());
            newTaskIdx = choices[randomIdx];
            newTask = taskPool[newTaskIdx];
        }

        TaskManager.instance.activeTasks[idxToAdd] = newTask;

        return newTask;
    }

    private void SpawnRandomTool()
    {
        if (ObjectManager.curTool == null)
        {
            var randTool = tools[Random.Range(0, tools.Count - 1)];
            var randToolItem = Instantiate(randTool, new Vector3(0, 0, 0), Quaternion.identity);
            randToolItem.GetComponent<Tool>()?.Equip(trashManiaDisplay);
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
            // Debug.Log("Knockable Progress");
            UpdateProgressForTasks(new TaskProgress(TaskProgress.TaskType.Knockable, obj));
        }
        else if (obj.GetComponent<Breakable>() != null)
        {
            UpdateProgressForTasks(new TaskProgress(TaskProgress.TaskType.Breakable, obj));
        }
    }

    private void UpdateProgressForTasks(TaskProgress progress)
    {
        foreach (var task in TaskManager.instance.activeTasks)
        {
            // Debug.Log("Update progress for task");
            Debug.Log(task.description);
            task.updateProgress(progress);
        }
    }

    private IEnumerator ShowObjectiveComplete(GameTask completedTask)
    {
        description.text = completedTask.description;
        objectiveComplete?.SetActive(true);
        // Debug.Log("Show Objective Complete");
        yield return new WaitForSeconds(2.0f);

        // while (description.color.a < 0.2)
        // {
        //     float alphaVal = Mathf.PingPong(Time.time / 1.0f, 1f);
        //     description.color = new Color(description.color.r, description.color.g, description.color.b, Mathf.Clamp(alphaVal, 0.0f, 1.0f));            
        //     yield return new WaitForSeconds(0.1f);
        // }

        objectiveComplete?.SetActive(false);
        //     Debug.Log("hide Objective Complete");
    }
    private IEnumerator ShowNewObjective(GameTask newTask)
    {
        if (newTask == null || SceneTransitionManager.instance.GetCurrentScene() != "MainScene") yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2.5f);
        newDescription.text = newTask.description;
        newObjective?.SetActive(true);
        // while (newDescription.color.a != 1)
        // {
        //     float alphaVal = 1f + Mathf.PingPong(Time.time / 0.5f, -1f);
        //     newDescription.color = new Color(newDescription.color.r, newDescription.color.g, newDescription.color.b, Mathf.Clamp(alphaVal, 0.0f, 1.0f));            
        //     yield return new WaitForSeconds(0.1f);
        // }
        // Debug.Log("Show new objective");
        yield return new WaitForSeconds(2.0f);
        newObjective?.SetActive(false);
        // Debug.Log("hide new objective");
    }

    public void Reset()
    {
        StopCoroutine("ShowObjectiveComplete");
        StopCoroutine("ShowNewObjective");

        foreach (var task in TaskManager.instance.taskPool)
        {
            // Debug.Log("Update progress for task");
            task.Reset();
        }

        // Renew active tasks
        TaskManager.instance.activeTasks = new List<GameTask>() { null, null, null };
        while (TaskManager.instance.activeTasks.Any(task => task == null))
            addRandomTask();

        TaskManager.instance.completedTasks.Clear();
        TaskManager.instance.pauseMenuTasks.Clear();
        TaskManager.instance.countdownTasks.Clear();
        TaskManager.instance.onScreenTasks.Clear();

        TaskManager.instance.linkedUI = false;
        TaskManager.instance.filledUI = false;

        TaskManager.instance.StartCoroutine("AddActiveTasksToUI");
        // Debug.Log("Dont need to add text is: " + addTextDoneCountdown);

    }

    public List<GameTask> GetCompletedTasks()
    {
        return completedTasks;
    }

    public void InitializeTasks()
    {
        if (!GameManager.instance.m_simplifyTasks)
        {
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
        }
        else
        {
            taskPool.Add(new KnockOverNItemsTask(1));
            taskPool.Add(new KnockOverNItemsTask(1));
            taskPool.Add(new KnockOverNItemsTask(1));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Ladder"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Ladder"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Brick"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Brick"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Wheelbarrow"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Wheelbarrow"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Paint Bucket"));
            taskPool.Add(new KnockOverNSpecificItemsTask(1, "Tool Box"));
        }

    }

    public void NotifyUIReady()
    {
        // AddActiveTasksToUI();

        TaskManager.instance.linkedUI = true;
    }

    private IEnumerator AddActiveTasksToUI()
    {
        Debug.Log(TaskManager.instance.linkedUI);
        yield return new WaitUntil(() => TaskManager.instance.linkedUI);
        yield return null;

        Debug.Log(TaskManager.instance.countdownTasks.Count); //
        Debug.Log(TaskManager.instance.pauseMenuTasks.Count); //
        Debug.Log(TaskManager.instance.onScreenTasks.Count);
        Debug.Log(TaskManager.instance.completedTasks.Count);
        for (int i = 0; i < TaskManager.instance.activeTasks.Count; i++)
        {
            Debug.Log(TaskManager.instance.activeTasks[i].description);
            Debug.Log(TaskManager.instance.pauseMenuTasks[i]);
            Debug.Log(TaskManager.instance.pauseMenuTasks[i].text);
            TaskManager.instance.pauseMenuTasks[i].text = TaskManager.instance.activeTasks[i].description;
            TaskManager.instance.countdownTasks[i].text = TaskManager.instance.activeTasks[i].description;
            TaskManager.instance.onScreenTasks[i].text = TaskManager.instance.activeTasks[i].description;
        }

        filledUI = true;
    }

}