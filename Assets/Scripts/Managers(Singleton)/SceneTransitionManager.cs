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

    private string currentScene; 

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
        currentScene = curScene.name;
        // Debug.Log("countdownDone is " + SceneTransitionManager.instance.countdownDone);
        if (!SceneTransitionManager.instance.countdownDone && countDownDisplay != null && curScene.name == GAME)
        {
            // Debug.Log("Started coroutinte");
            StartCoroutine(countDownDisplay.CountDown());
            SceneTransitionManager.instance.countdownDone = true;
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
        ScoreManager.instance?.Reset();
        ObjectManager.instance?.Reset();
        TimerManager.instance?.Reset();
        StartGameSteps();
        if (SceneManager.GetActiveScene().name != GAME) 
        {
            TaskManager.instance?.Reset();
            SceneManager.LoadScene(GAME);
        }
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
            SceneTransitionManager.instance.countdownDone = false;
            TimerManager.instance.StartTimer();
        } 
        else
        {
            SceneTransitionManager.instance.countdownDone = true;
        }

    }
    
    public void RegisterCountdownObj(CountDownDisplay display)
    {
        // Debug.Log("register countdown");
        countDownDisplay = display;
    }

    public string GetCurrentScene()
    {
        return currentScene;
    }
}