using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTips : MonoBehaviour
{
    [SerializeField] private GameObject upText;
    [SerializeField] private GameObject downText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(int floorNum)
    {
        // Debug.Log("[StairMenu] Show");
        this.gameObject.SetActive(true);
        if (floorNum == 1)
        {
            downText.SetActive(true);
        } else if (floorNum == 5)
        {
            upText.SetActive(true);
        } else {
	        upText.SetActive(true);
	        downText.SetActive(true);        	
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
