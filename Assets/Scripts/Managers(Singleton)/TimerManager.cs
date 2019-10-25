using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	
    public static TimerManager instance;

    public float totalTime;
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
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        timer = 0.0f;
        calledGameOver = false;
    }

    void Update()
    {
        if (timer < totalTime)
        {
            timer += Time.deltaTime / 2.0f;
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