using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    [SerializeField] private Text scoreText;

    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.instance.score;
    }
}
