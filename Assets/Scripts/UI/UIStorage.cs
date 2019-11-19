using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStorage : MonoBehaviour
{
	public StairTips stairTip;
	public BreakTips breakTip;

	public TextMeshProUGUI taskDisplay_CountDown_1;
	public TextMeshProUGUI taskDisplay_CountDown_2;
	public TextMeshProUGUI taskDisplay_CountDown_3;

	public TextMeshProUGUI taskDisplay_PauseMenu_1;
	public TextMeshProUGUI taskDisplay_PauseMenu_2;
	public TextMeshProUGUI taskDisplay_PauseMenu_3;

    public GameObject task_objectiveComplete;
    public TextMeshProUGUI task_description;
    public GameObject task_newObjective;
    public TextMeshProUGUI task_newDescription;
    public TrashManiaDisplay task_trashManiaDisplay;

    // Start is called before the first frame update
    void Start()
    {
        ObjectManager.instance.stairTips = stairTip;
        ObjectManager.instance.breakTip = breakTip;

        TaskManager.instance.countdownTasks.Add(taskDisplay_CountDown_1);
        TaskManager.instance.countdownTasks.Add(taskDisplay_CountDown_2);
        TaskManager.instance.countdownTasks.Add(taskDisplay_CountDown_3);

        TaskManager.instance.pauseMenuTasks.Add(taskDisplay_PauseMenu_1);
        TaskManager.instance.pauseMenuTasks.Add(taskDisplay_PauseMenu_2);
        TaskManager.instance.pauseMenuTasks.Add(taskDisplay_PauseMenu_3);

	    TaskManager.instance.objectiveComplete = task_objectiveComplete;
	    TaskManager.instance.description = task_description;
	    TaskManager.instance.newObjective = task_newObjective;
	    TaskManager.instance.newDescription = task_newDescription;
	    TaskManager.instance.trashManiaDisplay = task_trashManiaDisplay;

	    TaskManager.instance.NotifyUIReady();

    }
}
