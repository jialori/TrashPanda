using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStorage : MonoBehaviour
{
	public StairTips stairTip;
	public BreakTips breakTip;


    // Start is called before the first frame update
    void Awake()
    {
        ObjectManager.instance.stairTips = stairTip;
        ObjectManager.instance.breakTip = breakTip;

    }
}
