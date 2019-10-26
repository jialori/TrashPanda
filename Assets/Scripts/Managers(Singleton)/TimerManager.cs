using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	
    public static TimerManager instance;

    public float timeLeft;
    private float timer;    
    private bool calledGameOver;

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
        timer = timeLeft;
        calledGameOver = false;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (!calledGameOver)
        {
            SceneTransitionManager.instance.EndGame();
            calledGameOver = true;
        }
    }

    public float GetCurrentTime() 
    {
    	return timer;
    }

    public void Reset() 
    {
        instance.timer = 0.0f;
        calledGameOver = false;
    }

}