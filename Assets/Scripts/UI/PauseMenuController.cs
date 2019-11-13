using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject firstSelected;

    [SerializeField] private Button[] buttons;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Update()
    {
        if (Controller.GetPause())
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        GameManager.instance.TogglePlay();
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    // does not change the game's pause state
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    // does not change the game's pause state
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    // public void ResetPause(GameObject element)
    // public void Reset()
    // {
    //     EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
    //     EventSystem.current.SetSelectedGameObject(firstSelected);
    // }

}
