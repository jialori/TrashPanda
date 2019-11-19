using UnityEngine;

public class UIManager : MonoBehaviour
{
  // this class manages the displaying of UI elements on the screen in MainScene

  public static UIManager instance;

  [SerializeField] private GameObject uiCanvas; // should have Pause Menu, Main as children

  private GameObject pauseMenu;

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

    if (uiCanvas != null)
    {
      pauseMenu = uiCanvas.transform.GetChild(3).gameObject;
    }
  }

  public void ShowPauseMenu()
  {
    pauseMenu.SetActive(true);
  }

  public void HidePauseMenu()
  {
    pauseMenu.SetActive(false);
  }

  public void TogglePauseMenu()
  {
    pauseMenu.SetActive(!pauseMenu.activeSelf);
  }
}