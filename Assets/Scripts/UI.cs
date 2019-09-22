using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    RaccoonController racoon;
    private int food;
    private int maxFood;
    public Text foodDisplay;
    // Start is called before the first frame update
    void Start()
    {
        racoon = FindObjectOfType<RaccoonController>();
        food = racoon.food;
        maxFood = racoon.maxFood;
    }

    // Update is called once per frame
    void Update()
    {
        food = racoon.food;
        foodDisplay.text = "Food: " + food.ToString() + "\nFood needed: " + maxFood.ToString();
    }
}
