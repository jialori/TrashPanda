using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

public class TogglePlay : MonoBehaviour
{

    private EventSystem es;

    [Header("GameObject References")]
    public GameObject pauseMenu;
    public Text countDownText;

    bool _validInput;

    void Awake()
    {
        es = GetComponent<EventSystem>();
        _validInput = false;
    }

    void Update()
    {
        // Pause Menu
        if (Controller.GetPause()) //||
        // (Controller.GetA() && pauseMenu.activeSelf && es?.currentSelectedGameObject?.GetComponent<Button>()))
        {
            Debug.Log("TOggle play paused");
            _validInput = true;
        }

        if (_validInput)
        {
            GameManager.instance.TogglePlay();
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            _validInput = false;
        }
    }

}