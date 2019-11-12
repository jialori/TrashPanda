using UnityEngine;

public class TaskProgress
{
    public enum TaskType {
        Knockable,
        Breakable
    }

    public TaskType Type;
    public GameObject Object;

    public TaskProgress(TaskType type, GameObject obj)
    {
        Type = type;
        Object = obj;
    }

    public TaskProgress(TaskType type)
    {
        Type = type;
        Object = null;
    }
}