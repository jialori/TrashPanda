using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text foodDisplay;

    private RaccoonController racoon;
    private int food;
    private int maxFood;

    void Start()
    {
        racoon = FindObjectOfType<RaccoonController>();
        maxFood = racoon.maxFood;
        DisplayFoodText();
    }

    void Update()
    {
        DisplayFoodText();
    }

    void DisplayFoodText() {
        food = racoon.food;
        foodDisplay.text = "Food: " + food.ToString() + "\nFood needed: " + maxFood.ToString();
    }
}
