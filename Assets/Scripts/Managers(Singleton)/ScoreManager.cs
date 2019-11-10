using System;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
	
    public static ScoreManager instance;
    public float score;

    private List<BillEntry> billRecord = new List<BillEntry>();
    public BillDisplay billDisplay;
    public int scoreMultiplier = 1;

    public enum ActionTypes {BREAK, KNOCK};

    public class BillEntry
    {
        public string item;
        public ActionTypes aType;
        public float score;

        public BillEntry(string item, ActionTypes aType, float score)
        {
            this.item = item;
            this.aType = aType;
            this.score = score;
        }
    }


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
    	score = 0.0f;
    }

	public void AddScore(string item, ActionTypes aType, float n) {
		instance.score += n;
        billRecord.Add(new BillEntry(item, aType, n));
        billDisplay.AddBillLog(item, aType, n);
	}

	public void SubtractScore(float n) {
		instance.score -= n;
	}


    public void Reset() 
    {
        instance.score = 0.0f;
        instance.billRecord.Clear();
    }

}