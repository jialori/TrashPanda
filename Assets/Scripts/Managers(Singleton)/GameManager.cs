using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RaccoonController m_raccoon;
    public RaccoonController Raccoon { get => m_raccoon; }
    public static GameManager instance;

    [Header("Settings")]
    [SerializeField] public bool m_useController;
    public bool UseController { get => m_useController; }
    // true if wee start at MainScene or its copy directly (not transitioned from other scenes)
    // [SerializeField] public bool m_devMode = false;
    [SerializeField] public float totalTime;
    [SerializeField] public float m_volume;

    [Header("Debug Settings")]
    [SerializeField] public bool m_disableTimer = false;
    [SerializeField] public bool m_disableCountDown = false;
    [SerializeField] public bool m_simplifyTasks = false;

    // [HideInInspector] public List<HumanController> Workers = new List<HumanController>();
    [HideInInspector] public bool paused;

    [HideInInspector] public CentralHumanController CHC; // Reference to the Central Human Controller

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

    void Update()
    {
        if (!m_raccoon)
        {
            m_raccoon = GameObject.FindGameObjectWithTag("Raccoon")?.GetComponent<RaccoonController>();
        }

        if (Util.Controller.GetPause())
        {
            Debug.Log("Paused");
            TogglePlay();
            UIManager.instance.TogglePauseMenu();
        }
    }

    public void TogglePlay()
    {
        if (!m_disableTimer)
        {
            TimerManager.instance?.TogglePlay();
        }
        Raccoon?.TogglePlay();

        foreach (HumanController worker in GameManager.instance.CHC?.humans)
        {
            if (worker)
            {
                worker.TogglePlay();
            }
        }
        // TODO: Need to Toggle all the objects as well (potentially being knocked over)
    }
}