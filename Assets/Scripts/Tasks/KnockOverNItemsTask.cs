using UnityEngine;

public class KnockOverNItemsTask : GameTask
{
    private int numDestroyed;
    private int numGoal;
    public KnockOverNItemsTask(int num)
    {  
        numGoal = num;
        description = "Knock over " + num + " items";
    }

    public override bool isComplete()
    {
        return numDestroyed >= numGoal;
    }

    public override void updateProgress(TaskProgress progress)
    {
        if (progress.Type == TaskProgress.TaskType.Knockable)
        {
            numDestroyed ++;

            Debug.Log("progress: " + description);
        }
    }
}