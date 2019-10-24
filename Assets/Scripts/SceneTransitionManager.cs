using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    static public SceneTransitionManager instance;

    public string SCENE_NAME_GAME = "Level1_UI_Lori";
    public string SCENE_NAME_GAME_OVER = "GameOver";

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

    public void StartGame()
    {
        SceneManager.LoadScene(SCENE_NAME_GAME);
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(SCENE_NAME_GAME);
    }

    public void EndGame()
    {
        Debug.Log("EndGame called!");
        SceneManager.LoadScene(SCENE_NAME_GAME_OVER);
    }

}
