using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBerry : FoodSourceController
{
    // Start is called before the first frame update
    void Start()
    {
        m_NumFood = 4;
        Debug.Log("m_NumFood= " + m_NumFood);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
