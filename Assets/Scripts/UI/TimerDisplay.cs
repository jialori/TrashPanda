using UnityEngine;
using System.Collections;
using TMPro;

public class TimerDisplay: MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
    	StartCoroutine(UpdateTimerDisplay());
    }

    IEnumerator UpdateTimerDisplay()
    {
        var time = TimerManager.instance.GetCurrentTime();
    	while (true)
    	{
            time = TimerManager.instance.GetCurrentTime();
	        timerText.text = Util.Util.GetFormattedTime(time);
	        yield return new WaitForSeconds(1f);
    	}
    }

}
