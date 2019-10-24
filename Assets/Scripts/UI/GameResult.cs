using UnityEngine;
using UnityEngine.UI;
using System;

public class GameResult : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text trashBinScoreText;

    // Update is called once per frame
    void Update()
    {
    	float score = ScoreManager.instance.score;
        scoreText.text = "You Earned $" + score;
        trashBinScoreText.text = "which is worth " + Math.Floor(score / 50.0f) + " trash bins :)";
    }
}
