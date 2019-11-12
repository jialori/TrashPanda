using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "$" + ScoreManager.instance.score * ScoreManager.instance.scoreMultiplier;
    }
}
