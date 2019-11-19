using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterToTaskManagerOnScreenTasks : MonoBehaviour
{
  // Start is called before the first frame update
  void Awake()
  {
    TaskManager.instance?.onScreenTasks.Add(this.gameObject.GetComponent<TextMeshProUGUI>());
    // Debug.Log("Registered to countDown");
  }
}