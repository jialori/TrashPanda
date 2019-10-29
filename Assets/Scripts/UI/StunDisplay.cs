using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunDisplay : MonoBehaviour
{
    [SerializeField] private Text stunText;
    [SerializeField] RaccoonController raccoon;

    void Start()
    {
        //raccoon = GameObject.Find("Raccoon").GetComponent<RaccoonController>();
    }

    void Update()
    {
        if (raccoon.isStunned)
        {
            stunText.enabled = true;
        }
        else
        {
            stunText.enabled = false;
        }
    }
}
