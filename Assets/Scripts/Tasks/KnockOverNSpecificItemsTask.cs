using UnityEngine;

public class KnockOverNSpecificItemsTask : GameTask
{
    private int numDestroyed;
    private int numGoal;
    private string itemType;

    public KnockOverNSpecificItemsTask(int num, string itemType)
    {  
        numGoal = num;
        description = "Knock over " + num + " " + itemType + "s";

    }

    public override bool isComplete()
    {
        return numDestroyed >= numGoal;
    }

    public override void updateProgress(TaskProgress progress)
    {
        if (progress.Type == TaskProgress.TaskType.Knockable)
        {
            if (progress.Object != null && progress.Object.GetComponent<Knockable>().name == itemType)
            {
                numDestroyed ++;
                Debug.Log("progress: " + description);
            }
        }
    }
}