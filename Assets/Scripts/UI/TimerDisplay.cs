using UnityEngine;
using TMPro;

public class TimerDisplay: MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        var time = TimerManager.instance.GetCurrentTime();
        var mins = Mathf.Ceil(time / 60);
        var secs = time - mins * 60;
        timerText.text = mins + " min " + secs + " sec";
    }

}
