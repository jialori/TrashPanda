using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    static public SceneTransitionManager instance;

    [Header("Scene Names")]
    public string MENU;
    public string GAME;
    public string GAME_OVER;

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

    public void Menu()
    {
        ScoreManager.instance.Reset();
        TimerManager.instance.Reset();
        SceneManager.LoadScene(MENU);
    }

    public void StartGame()
    {
        ScoreManager.instance.Reset();
        TimerManager.instance.Reset();
        SceneManager.LoadScene(GAME);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(GAME_OVER);
    }

}
