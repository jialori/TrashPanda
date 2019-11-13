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
    	while (true)
    	{
	        var time = TimerManager.instance.GetCurrentTime();
	        timerText.text = Util.Util.GetFormattedTime(time);
	        yield return new WaitForSeconds(1f);
    	}
    }

}
