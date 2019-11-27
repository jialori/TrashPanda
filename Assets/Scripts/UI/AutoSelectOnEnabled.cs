using System.Collections;
using UnityEngine.EventSystems;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine;

public class AutoSelectOnEnabled : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null); // fix Unity highlighting
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

}
