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
        // EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
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

    // public void ResetPause(GameObject element)
    public void Reset()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
    }

}
