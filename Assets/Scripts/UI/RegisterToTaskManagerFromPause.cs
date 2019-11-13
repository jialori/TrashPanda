using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterToTaskManagerFromPause : MonoBehaviour
{
    void Awake()
    {
        // Debug.Log("Registered to pauseMenu");
        TaskManager.instance.pauseMenuTasks.Add(this.gameObject.GetComponent<TextMeshProUGUI>());
    }

}
