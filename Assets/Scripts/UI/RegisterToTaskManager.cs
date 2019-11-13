using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterToTaskManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TaskManager.instance.countdownTasks.Add(this.gameObject.GetComponent<TextMeshProUGUI>());
        // Debug.Log("Registered to countDown");
    }
}
