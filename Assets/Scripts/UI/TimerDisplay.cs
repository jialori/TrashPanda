using UnityEngine;
using TMPro;

public class TimerDisplay: MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        var time = TimerManager.instance.GetCurrentTime();
        var mins = Mathf.Floor(time / 60);
        var secs = time % 60;
        timerText.text = string.Format("{0:00} min {1:00} sec", mins, secs);
    }

}
