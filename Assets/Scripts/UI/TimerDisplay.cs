using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerDisplay: MonoBehaviour
{

	[SerializeField] private Text timerText;

    void Update()
    {
        timerText.text = "Time: " + TimerManager.instance.GetCurrentTime().ToString("F");
    }

}
