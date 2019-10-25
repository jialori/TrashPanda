using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool m_useController;
    public bool UseController { get => m_useController; }
    [SerializeField] private RaccoonController m_raccoon;
    public RaccoonController Raccoon { get => m_raccoon; }
	
    public static GameManager instance;
    private static AudioManager audioManager;
    private static ObjectManager objectManager;
    private static ScoreManager scoreManager;
    private static TimerManager timerManager;

    private static SceneTransitionManager sceneTransitionManager;

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

    void Start() 
    {
        // Initialize all managers
        audioManager = AudioManager.instance;
        objectManager = ObjectManager.instance;
        scoreManager = ScoreManager.instance;
        timerManager = TimerManager.instance;
        sceneTransitionManager = SceneTransitionManager.instance;
    }
}