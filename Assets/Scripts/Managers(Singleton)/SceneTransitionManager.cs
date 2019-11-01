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

    void Start()
    {
        if (SceneManager.GetActiveScene().name == MENU)
        {
        // play BGM when start at Menu
        AudioManager.instance.Play("MainMenuBGM");
        }
    }

    public void Menu()
    {
        AudioManager.instance.StopCurrent();
        SceneManager.LoadScene(MENU);
        AudioManager.instance.Play("MainMenuBGM");
    }

    public void StartGame()
    {
        Debug.Log("[SceneManager] StartGame");
        AudioManager.instance.StopCurrent();
        GameManager.instance.Reset();
        SceneManager.LoadScene(GAME);
        GameManager.instance.StartGame();
    }

    public void EndGame()
    {
        Debug.Log("[SceneManager] EndGame");
        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("MainMenuBGM");
        SceneManager.LoadScene(GAME_OVER);
    }

    public void QuitGame()
    {
        // Debug.Log("Quit");
        Application.Quit();
    }

}