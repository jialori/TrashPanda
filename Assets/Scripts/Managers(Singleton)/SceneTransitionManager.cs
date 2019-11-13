using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    static public SceneTransitionManager instance;

    [Header("Scene Names")]
    public string MENU;
    public string GAME;
    public string GAME_OVER;

    [Header("References")]
    [SerializeField] private CountDownDisplay countDownDisplay;
    private bool countdownDone = false;

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
        else if (SceneManager.GetActiveScene().name == GAME)
        {
            // GameManager.instance.StartGame();
            // StartGameSteps();
            StartGame();
        }
    }

    void Update()
    {
        var curScene = SceneManager.GetActiveScene();
        Debug.Log(curScene.name);
        if (!countdownDone && countDownDisplay != null && curScene.name == GAME)
        {
            StartCoroutine(countDownDisplay.CountDown());
            countdownDone = true;
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
        // Debug.Log("[SceneManager] StartGame");
        AudioManager.instance.StopCurrent();
        GameManager.instance.Reset();
        if (SceneManager.GetActiveScene().name != GAME) SceneManager.LoadScene(GAME);
        StartGameSteps();
    }

    public void EndGame()
    {
        // Debug.Log("[SceneManager] EndGame");
        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("MainMenuBGM");
        SceneManager.LoadScene(GAME_OVER);
    }

    public void QuitGame()
    {
        // Debug.Log("Quit");
        Application.Quit();
    }


    void StartGameSteps()
    {
        if (!GameManager.instance.m_disableCountDown)
        {
            countdownDone = false;
        } 
        else
        {
            countdownDone = true;
            TimerManager.instance.StartTimer();
        }

    }
    
    public void RegisterCountdownObj(CountDownDisplay display)
    {
        Debug.Log("register countdown");
        countDownDisplay = display;
    }
}