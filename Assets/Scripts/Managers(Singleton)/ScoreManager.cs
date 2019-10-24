using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	
    public static ScoreManager instance;

    public float score;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
    	score = 0.0f;
    }

	public void AddScore(float n) {
		instance.score += n;
	}

	public void SubtractScore(float n) {
		instance.score -= n;
	}

}