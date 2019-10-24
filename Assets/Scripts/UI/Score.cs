using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public RaccoonController raccoonController;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.instance.score;
    }
}
