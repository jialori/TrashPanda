using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;

public class TogglePlay : MonoBehaviour
{

    public GameObject pauseMenu;
    public EventSystem es;

    void Start()
    {
        es = GetComponent<EventSystem>();
    }

    void Update()
    {
        bool _validInput = false;
        if (Controller.GetPause() ||
            (Controller.GetA() && pauseMenu.activeSelf && es?.currentSelectedGameObject?.GetComponent<Button>()))
        {
            _validInput = true;
        }

        if (_validInput)
        {
            GameManager.instance.TogglePlay();
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

}