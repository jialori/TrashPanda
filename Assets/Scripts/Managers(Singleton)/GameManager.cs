using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool m_useController;
    public bool UseController { get => m_useController; }

    [SerializeField] private RaccoonController m_raccoon;
    public RaccoonController Raccoon { get => m_raccoon; }
    public static GameManager instance;

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

    void Start() { }

    public void StartGame()
    {
        TimerManager.instance.StartTimer();
    }

    public void Reset()
    {
        ScoreManager.instance?.Reset();
        ObjectManager.instance?.Reset();
        TimerManager.instance?.Reset();
    }
}