using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarRaccoon : MonoBehaviour
{
    public Image BarImg; // The Status bar image
    public Text BarText; // Percentage Text on the Status Bar
    public int Max; // Maximum Fullness Value
    public int autoDecSpeed;
    public float autoDecTimeGap;

    private int m_CurValue;
    private float m_CurPercent;
    float timeLeft;

    private void SetValue(int newValue)
    {
        if (m_CurValue != newValue)
        {
            // Set fullness value and percentage of the status bar.
            if (Max == 0)
            {
                m_CurValue = 0;
                m_CurPercent = 0;
            }
            else
            {
                if (newValue < 0)
                {
                    newValue = 0;
                }
                m_CurValue = newValue;
                m_CurPercent = (float)newValue / (float)Max;
            }
            // Update Image and Text on the Status bar
            BarText.text = string.Format("{0} %", Mathf.Round(m_CurPercent * 100));
            BarImg.fillAmount = m_CurPercent;
        }
    }


    // Set Status to Max at the beginning of the game
    private void Start()
    {
        SetValue(Max);
        timeLeft = autoDecTimeGap;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            SetValue(m_CurValue - autoDecSpeed);
            timeLeft = autoDecTimeGap;
        }
        if (m_CurValue <= 0)
        {
            // TODO GameEnding Scripts
            // Game Over
        }
    }


    //
    // Below are Functions and Accessors that can be accessed from outside of this script
    //

    // Accessor for current percentage of the status bar
    public float CurPercent
    {
        get { return m_CurPercent; }
    }

    // Accessor for current value of the status bar
    public float CurValue
    {
        get { return m_CurValue; }
    }

    // To increase the value when eating food 
    // or decrease (by setting addVal to negative) when encounter some dangers
    public void addValue(int addVal)
    {
        SetValue(m_CurValue + addVal);
    }
}
