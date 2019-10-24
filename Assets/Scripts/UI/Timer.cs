using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer: MonoBehaviour
{
    [SerializeField] private float timeLeft;
    [SerializeField] private Text timerText;
    // public string sceneName;

    private RaccoonController raccoon;
    private float timer;

    void Start()
    {
        // raccoon = FindObjectOfType<RaccoonController>();
        // timer = 0;
    }

    void Update()
    {
        // if (timer < timeLeft)
        // {
        //     timer += Time.deltaTime / 2.0f;
            // timerText.text = "Time: " + timer.ToString("F");
        timerText.text = "Time: " + TimerManager.instance.GetCurrentTime().ToString("F");
        // }
        // else
        // {
        //     SceneManager.LoadScene(sceneName);
        // }
    }
}
