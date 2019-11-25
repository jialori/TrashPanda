using UnityEngine;
using System.Collections;
using TMPro;

public class TimerDisplay: MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI timerText;

    void OnEnable()
    {
    	StartCoroutine(UpdateTimerDisplay());
    }

    IEnumerator UpdateTimerDisplay()
    {
        yield return new WaitUntil(()=>TimerManager.instance != null);

        var time = TimerManager.instance.GetCurrentTime();
    	while (true)
    	{
            time = TimerManager.instance.GetCurrentTime();
	        timerText.text = Util.Util.GetFormattedTime(time);
	        yield return new WaitForSeconds(1f);
    	}
    }

}
