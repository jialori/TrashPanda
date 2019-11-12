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
    	while (true)
    	{
	        var time = TimerManager.instance.GetCurrentTime();
	        var mins = Mathf.Floor(time / 60);
	        var secs = time % 60;
	        timerText.text = string.Format("{0:00} min {1:00} sec", mins, secs);
	        yield return new WaitForSeconds(1f);
    	}
    }

}
