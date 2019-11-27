﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEditor;

public class SceneTransitionManager : MonoBehaviour
{

    static public SceneTransitionManager instance;

    [Header("Scene Names")]
    public string MENU;
    public string GAME;
    public string GAME_OVER;

    // Links
    [HideInInspector] public CountDownDisplay countDownDisplay;
    [HideInInspector] public GameObject GAME_UI;
    [HideInInspector] public GameObject GAME_CAMERA;
    
    private bool gameOn = false;
    private string currentScene; 

    private bool hasLinkedUI = false;
    private bool hasLinkedCamera = false;

    void Awake()
    {
        if (instance != null)
        {
            // Debug.Log("Destroyed sce")
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {

        SceneManager.LoadScene(GAME);
        SceneManager.LoadScene(MENU, LoadSceneMode.Additive);
        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedUI,
                ()=>SceneTransitionManager.instance.GAME_UI.SetActive(false) ));


        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedCamera,
                ()=>SceneTransitionManager.instance.GAME_CAMERA.SetActive(false) ));

        SceneTransitionManager.instance.gameOn = false;
        SceneTransitionManager.instance.currentScene = MENU;

        AudioManager.instance.Play("MainMenuBGM");

        // if (SceneManager.GetActiveScene().name == MENU)
        // {
        //     // play BGM when start at Menu
        //     AudioManager.instance.Play("MainMenuBGM");
        // } 
        // else if (SceneManager.GetActiveScene().name == GAME)
        // {
        //     // GameManager.instance.StartGame();
        //     // StartGameSteps();
        //     StartGame();
        // }
    }


    public void Menu()
    {
        SceneManager.LoadScene(MENU, LoadSceneMode.Additive);

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedUI,
                ()=>SceneTransitionManager.instance.GAME_UI.SetActive(false) ));

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedCamera,
                ()=>SceneTransitionManager.instance.GAME_CAMERA.SetActive(false) ));
        
        SceneTransitionManager.instance.currentScene = MENU;

        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("MainMenuBGM");

        SceneTransitionManager.instance.gameOn = false;
    }

    public void StartGameOnMainMenu()
    {
        // Debug.Log("[SceneManager] StartGame");
        SceneTransitionManager.instance.StartCoroutine(StartGameSteps());

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedUI,
                ()=>SceneTransitionManager.instance.GAME_UI.SetActive(true) ));

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedCamera,
                ()=>SceneTransitionManager.instance.GAME_CAMERA.SetActive(true) ));

        SceneTransitionManager.instance.currentScene = GAME;

        // ScoreManager.instance?.Reset();
        // ObjectManager.instance?.Reset();
        // TimerManager.instance?.Reset();
        // TaskManager.instance?.Reset();

        SceneTransitionManager.instance.gameOn = true;

        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("ThemeSong");
    }


    public void StartGameOnGameOver()
    {
        // Debug.Log("[SceneManager] StartGame");
        SceneTransitionManager.instance.StartCoroutine(StartGameSteps());

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedUI,
                ()=>SceneTransitionManager.instance.GAME_UI.SetActive(true) ));

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedCamera,
                ()=>SceneTransitionManager.instance.GAME_CAMERA.SetActive(true) ));

        SceneTransitionManager.instance.currentScene = GAME;

        ScoreManager.instance?.Reset();
        ObjectManager.instance?.Reset();
        TimerManager.instance?.Reset();
        TaskManager.instance?.Reset();

        SceneTransitionManager.instance.gameOn = true;

        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("ThemeSong");
    }


    public void EndGame()
    {
        // Debug.Log("[SceneManager] EndGame");
        GameManager.instance.Raccoon.Reset(); // refactor: maybe broadcast event to all gameobjects & they do their own thing
        SceneManager.LoadScene(GAME_OVER, LoadSceneMode.Additive);
        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedUI,
                ()=>SceneTransitionManager.instance.GAME_UI.SetActive(false) ));

        SceneTransitionManager.instance.StartCoroutine(
            DoAfter( 
                ()=>SceneTransitionManager.instance.hasLinkedCamera,
                ()=>SceneTransitionManager.instance.GAME_CAMERA.SetActive(false) ));

        SceneTransitionManager.instance.currentScene = GAME_OVER;

        AudioManager.instance.StopCurrent();
        AudioManager.instance.Play("MainMenuBGM");

        SceneTransitionManager.instance.gameOn = false;
    }

    public void QuitGame()
    {
        // Debug.Log("Quit");
        Application.Quit();
    }


    public bool isGameOn()
    {
        return SceneTransitionManager.instance.gameOn;
    }

    public string GetCurrentScene()
    {
        return SceneTransitionManager.instance.currentScene;
    }



    public void NotifyUIReady()
    {
        SceneTransitionManager.instance.hasLinkedUI = true;
    }

    public void NotifyCameraReady()
    {
        SceneTransitionManager.instance.hasLinkedCamera = true;
    }


// ==============================================
// ========== Helper Functions START ============
// ==============================================

    IEnumerator StartGameSteps()
    {
        // Load the GAME scene if it is not loaded
        if (!SceneManager.GetSceneByName(GAME).isLoaded) { SceneManager.LoadScene(GAME); }

        // Unload the current scene if it is not GAME
        if (SceneTransitionManager.instance.currentScene != GAME)
        {
            AsyncOperation unloadInfo = SceneManager.UnloadSceneAsync(SceneTransitionManager.instance.currentScene);
            yield return new WaitUntil(() => unloadInfo.isDone);     
        }

        // Start the timer or count-down
        if (GameManager.instance.m_disableCountDown)
        {
            TimerManager.instance.StartTimer();
        } 
        else
        {
            yield return new WaitUntil(() => SceneTransitionManager.instance.currentScene == GAME);
            yield return new WaitUntil(() => SceneTransitionManager.instance.countDownDisplay != null);

            SceneTransitionManager.instance.StartCoroutine(SceneTransitionManager.instance.countDownDisplay.CountDown());
        }
    }

    // Execute action once the waitFor function returns true 
    IEnumerator DoAfter(Func<bool> waitFor, Action action)
    {
        yield return new WaitUntil(waitFor);
        action();
    }



}