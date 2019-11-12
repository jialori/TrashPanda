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

	// void Start()
	// {
        // Count Down on Start
        // countDownText.GetComponent<CountDownDisplay>().StartCoroutine("CountDown");
        // StartCoroutine("CountDown");
	// }

    public IEnumerator CountDown()
    {
    	TimerManager.instance.StopTimer();
        GameManager.instance.Raccoon.TogglePlay();
        countDownDisplay.SetActive(true);

        for (int t = 3; t > 0; t--)
        {
            countDownText.text = t.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        
        countDownText.text = "START!";
        yield return new WaitForSeconds(1.0f);

        countDownDisplay.SetActive(false);
        GameManager.instance.Raccoon.TogglePlay();
    	TimerManager.instance.StartTimer();
    }
}
