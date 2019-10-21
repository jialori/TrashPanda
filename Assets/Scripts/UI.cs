using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI: MonoBehaviour
{
    public Text foodDisplay;

    private RaccoonController raccoon;
    private int food;
    private int maxFood;

    void Start()
    {
        raccoon = FindObjectOfType<RaccoonController>();
        //maxFood = raccoon.GetMaxFood();
        DisplayFoodText();
    }

    void Update()
    {
        DisplayFoodText();
    }

    void DisplayFoodText() {
        //food = raccoon.GetFood();
        //foodDisplay.text = "Food carrying: " + food.ToString() + "/" + maxFood.ToString();
    }
}
