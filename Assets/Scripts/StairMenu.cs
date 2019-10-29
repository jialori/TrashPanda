using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StairMenu : MonoBehaviour
{

    [SerializeField] private Text upText;
    [SerializeField] private Text downText;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(int floorNum)
    {
        Debug.Log("[StairMenu] Show");
        this.upText.enabled = true;
        this.downText.enabled = true;
        this.gameObject.SetActive(true);
        if (floorNum == 1)
        {
            this.downText.enabled = false;
        }
        if (floorNum == 5)
        {
            this.upText.enabled = false;
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}