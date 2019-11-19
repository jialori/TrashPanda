using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTips : MonoBehaviour
{
    [SerializeField] private GameObject upText;
    [SerializeField] private GameObject downText;

    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(int floorNum)
    {
        // Debug.Log("[StairMenu] Show");
        this.gameObject.SetActive(true);
        upText.SetActive(true);
        downText.SetActive(true);           
        if (floorNum == 1)
        {
            downText.SetActive(false);
        } 
        else if (floorNum == 5)
        {
            upText.SetActive(false);
        } 

    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
