using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTips : MonoBehaviour
{
    // [SerializeField] private GameObject breakTip;

    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        // Debug.Log("[StairMenu] Show");
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
