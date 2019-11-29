using UnityEngine;

public class KnockOverNItemsTask : GameTask
{
    private int numDestroyed;
    private int numGoal;
    public KnockOverNItemsTask(int num)
    {
        numGoal = num;
        initialDescription = "Knock over " + num + " items";
        description = initialDescription;
    }

    public override bool isComplete()
    {
        return numDestroyed >= numGoal;
    }

    public override void updateProgress(TaskProgress progress)
    {
        if (progress.Type == TaskProgress.TaskType.Knockable && numDestroyed != numGoal)
        {
            numDestroyed++;
            description = "Knock over " + numDestroyed + "/" + numGoal + " items";

            //Debug.Log("progress: " + description);
        }
    }

    public override void Reset()
    {
        numDestroyed = 0;
        description = initialDescription;
    }
}