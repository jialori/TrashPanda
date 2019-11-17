using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager instance;

    private float timer;
    [System.NonSerialized] public bool timerOn = false;

    public string sceneName;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        timerOn = false;
    }

    void Start()
    {
        timer = GameManager.instance.totalTime;
    }

    void Update()
    {
        if (timerOn)
        {
            Debug.Log("timer on");
            if (timer > 0) timer = Mathf.Max(timer - Time.deltaTime, 0);
            else
            {
                SceneTransitionManager.instance.EndGame();
                timerOn = false;
            }
        }
    }

    public void StartTimer()
    {
        if (!GameManager.instance.m_disableTimer)
        {
            timerOn = true;
        }
    }

    public void StopTimer()
    {
        timerOn = false;
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    public void Reset()
    {
        instance.timer = GameManager.instance.totalTime;
        timerOn = false;
    }

    public void TogglePlay()
    {
        timerOn = !timerOn;
    }

}