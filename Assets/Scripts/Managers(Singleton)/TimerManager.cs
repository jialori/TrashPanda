using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager instance;

    [SerializeField] private float totalTime;
    private float timer;
    private bool timerOn;

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
    }

    void Start()
    {
        timer = totalTime;
        timerOn = false;
    }

    void Update()
    {
        if (timerOn)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                SceneTransitionManager.instance.EndGame();
                timerOn = false;
            }
        }
    }

    public void StartTimer()
    {
        timerOn = true;
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    public void Reset()
    {
        instance.timer = totalTime;
        timerOn = false;
    }

}