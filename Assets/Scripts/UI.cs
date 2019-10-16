using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI: MonoBehaviour
{
    [SerializeField] private float timeLeft;
    [SerializeField] private Text timerText;
    public string sceneName;

    private RaccoonController raccoon;
    private float timer;

    void Start()
    {
        raccoon = FindObjectOfType<RaccoonController>();
        timer = timeLeft;
    }

    void Update()
    {
        if (timeLeft > 0.0f)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time left: " + timeLeft.ToString("F");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
