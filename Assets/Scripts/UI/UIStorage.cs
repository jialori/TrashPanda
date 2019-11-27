using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStorage : MonoBehaviour
{
    public StairTips stairTip;
    public BreakTips breakTip;

    [SerializeField] private TextMeshProUGUI[] taskDisplayCountdown;
    [SerializeField] private TextMeshProUGUI[] taskDisplayPauseMenu;
    [SerializeField] private TextMeshProUGUI[] taskDisplayOnScreen;

    public GameObject task_objectiveComplete;
    public TextMeshProUGUI task_description;
    public GameObject task_newObjective;
    public TextMeshProUGUI task_newDescription;
    public TrashManiaDisplay task_trashManiaDisplay;


    public GameObject canvas;
    public GameObject mainCamera;
    public CountDownDisplay countDownDisplay;


    // Start is called before the first frame update
    void Start()
    {
        ObjectManager.instance.stairTips = stairTip;
        ObjectManager.instance.breakTip = breakTip;

        TaskManager.instance.countdownTasks.AddRange(taskDisplayCountdown);

        TaskManager.instance.pauseMenuTasks.AddRange(taskDisplayPauseMenu);

        TaskManager.instance.onScreenTasks.AddRange(taskDisplayOnScreen);

        TaskManager.instance.objectiveComplete = task_objectiveComplete;
        TaskManager.instance.description = task_description;
        TaskManager.instance.newObjective = task_newObjective;
        TaskManager.instance.newDescription = task_newDescription;
        TaskManager.instance.trashManiaDisplay = task_trashManiaDisplay;
        TaskManager.instance.NotifyUIReady();

        SceneTransitionManager.instance.GAME_UI = canvas;
        SceneTransitionManager.instance.NotifyUIReady();
        SceneTransitionManager.instance.GAME_CAMERA = mainCamera;
        SceneTransitionManager.instance.NotifyCameraReady();
        SceneTransitionManager.instance.countDownDisplay = countDownDisplay;

    }
}