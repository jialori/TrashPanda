using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public string sceneName = "MainScene";
    public void RetryGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
