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
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(MENU);
    }

    public void StartGame()
    {
        Debug.Log("[SceneManager] StartGame");
        GameManager.instance.Reset();
        SceneManager.LoadScene(GAME);
        GameManager.instance.StartGame();
    }

    public void EndGame()
    {
        Debug.Log("[SceneManager] EndGame");
        SceneManager.LoadScene(GAME_OVER);
    }

}