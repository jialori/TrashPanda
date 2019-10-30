using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        if (GetTogglePlay() ||
            (GetA() && pauseMenu.activeSelf && es?.currentSelectedGameObject?.GetComponent<Button>()))
        {
            _validInput = true;
        }

        if (_validInput)
        {
            GameManager.instance.TogglePlay();
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    private bool GetTogglePlay()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("Pause");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    private bool GetA()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}