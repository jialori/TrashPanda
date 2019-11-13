// MIT license - mit-license.txt
// Alpha Flashing Text v2.0 - https://github.com/nvjob/alpha-flashing-text-simple
// #NVJOB Nicholas Veselov (independent developer) - https://nvjob.github.io

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Util;

// This file only handles max ONE TMP object and ONE Text object
public class ButtonFlashingEffect : MonoBehaviour
{
    public Text tarText;
    public TextMeshProUGUI tarTextTMP;
    public float minAlpha = 0.0f;
    public float maxAlpha = 1f;
    public float timerAlpha = 5.0f;
    float alphaVal;
    Color initColor;
    Color initColorTMP;

    bool isActive = false;
    bool isDirty = false;

    void Awake()
    {
        // isActive = ;
    }

    void Update()
    {
        if (Controller.GetXAxis() > 0 || Controller.GetYAxis() > 0)
        {
            isDirty = true;
        }
    }

    void LateUpdate()
    {
        if (!isActive) return;
        
        // tarTextTMP = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (tarTextTMP)
        {
            alphaVal = minAlpha + Mathf.PingPong(Time.time / timerAlpha, maxAlpha - minAlpha);
            tarTextTMP.color = new Color(tarTextTMP.color.r, tarTextTMP.color.g, tarTextTMP.color.b, Mathf.Clamp(alphaVal, 0.0f, 1.0f));
        }

        // tarText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        if (tarText)
        {
            alphaVal = minAlpha + Mathf.PingPong(Time.time / timerAlpha, maxAlpha - minAlpha);
            tarText.color = new Color(tarText.color.r, tarText.color.g, tarText.color.b, Mathf.Clamp(alphaVal, 0.0f, 1.0f));
        }
    }

    void NotifyChange()
    {
        // Resume the previously selected object's alpha value 
        if (tarTextTMP) {tarTextTMP.color = new Color(tarTextTMP.color.r, tarTextTMP.color.g, tarTextTMP.color.b, 1f);}
        if (tarText) {tarText.color = new Color(tarText.color.r, tarText.color.g, tarText.color.b, 1f);}
        isActive = true;

        tarTextTMP = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        tarText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        if (tarTextTMP)
        {
            initColorTMP = tarTextTMP.color;             
        }
        if (tarText)
        {
            initColor = tarText.color;        
        }
    }

}
