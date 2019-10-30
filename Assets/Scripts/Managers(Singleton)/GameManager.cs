using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool m_useController;
    public bool UseController { get => m_useController; }
    // true if wee start at MainScene or its copy directly (not transitioned from other scenes)
    // [SerializeField] public bool m_devMode = false;
    [SerializeField] public bool m_disableTimer = false;

    [SerializeField] private RaccoonController m_raccoon;
    public RaccoonController Raccoon { get => m_raccoon; }
    public List<HumanController> Workers = new List<HumanController>();
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

    void Start() {}

    public void StartGame()
    {
        if (!m_disableTimer)
        {
            TimerManager.instance.StartTimer();
        }
    }

    public void Reset()
    {
        ScoreManager.instance?.Reset();
        ObjectManager.instance?.Reset();
        TimerManager.instance?.Reset();
    }

    public void TogglePlay()
    {
        if (!m_disableTimer)
        {
            TimerManager.instance?.TogglePlay();
        }
        Raccoon?.TogglePlay();
        foreach(HumanController worker in Workers) {
            if (worker)
            {
              worker.TogglePlay();
            }
        }
        // TODO: Need to Toggle all the objects as well (potentially being knocked over)
    }
}