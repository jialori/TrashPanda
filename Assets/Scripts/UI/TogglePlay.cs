using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlay : MonoBehaviour
{

	public GameObject pauseMenu;

    void Update()
    {
		if (GetTogglePlay())
    	{
    		GameManager.instance.TogglePlay();
    		pauseMenu.SetActive(!pauseMenu.activeSelf);
    	}
    }

    private bool GetTogglePlay()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("Submit");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.P);
        }
    }
}
