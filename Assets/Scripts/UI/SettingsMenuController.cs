using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    public PauseMenuController pauseMenuController;
    public GameObject firstSelected;

    public Toggle useMouseToggle;
    public Toggle useControllerToggle;

    void OnEnable()
    {
        // EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Start()
    {
        // Volume slider

        // Device toggles
        if (GameManager.instance.m_useController)
        {
            useControllerToggle.isOn = true;
        } else {
            useMouseToggle.isOn = true;
        }
    }

    public void useController()
    {
        GameManager.instance.m_useController = true;
    }

    public void useMouse()
    {
        GameManager.instance.m_useController = false;
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
        settingsMenu.SetActive(false);
    }

    public void AdjustGlobalVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
        GameManager.instance.m_volume = newVolume; // update volume globally
    }

}
