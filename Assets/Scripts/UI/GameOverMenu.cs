using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public string MainScene = "MainScene";
    public string MainMenu = "MainMenu";

    public void RetryGame()
    {
        SceneManager.LoadScene(MainScene);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
