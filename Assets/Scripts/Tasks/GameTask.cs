using UnityEngine;

public abstract class GameTask
{
    public string description;

    public abstract bool isComplete();

    public abstract void updateProgress(TaskProgress progress);

    public abstract void Reset();
}
