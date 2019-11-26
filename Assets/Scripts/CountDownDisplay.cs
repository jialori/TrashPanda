using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CountDownDisplay : MonoBehaviour
{

	private Text countDownText;
	private GameObject countDownDisplay;

	void Awake()
	{
		countDownDisplay = this.gameObject;
		countDownText = GetComponent<Text>();
	}

    public IEnumerator CountDown()
    {
        // Debug.Log("Count down routine called.");
    	TimerManager.instance.StopTimer();
        GameManager.instance.Raccoon?.Pause();
        countDownDisplay.SetActive(true);

        for (int t = 3; t > 0; t--)
        {
            countDownText.text = t.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        
        countDownText.text = "START!";
        transform.GetChild(0).gameObject.SetActive(false);
        
        GameManager.instance.Raccoon?.UnPause();
        TimerManager.instance.StartTimer();
        
        yield return new WaitForSeconds(1.0f);
        countDownDisplay.SetActive(false);
    }
}
