using UnityEngine;

public class KnockOverNSpecificItemsTask : GameTask
{
    private int numDestroyed;
    private int numGoal;
    private string itemType;

    public KnockOverNSpecificItemsTask(int num, string itemType)
    {
        numGoal = num;
        this.itemType = itemType;
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
            // if (progress.Object != null)
            // Debug.Log(progress.Object.GetComponent<Knockable>().name);
            // Debug.Log(this.itemType);
            if (progress.Object != null && progress.Object.GetComponent<Knockable>().objName == itemType)
            {
                numDestroyed++;
                Debug.Log("progress: " + description);
            }
        }
    }

    public override void Reset()
    {
        numDestroyed = 0;
    }

    public string getItemType()
    {
        return itemType;
    }
}