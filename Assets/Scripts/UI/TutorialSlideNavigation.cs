using UnityEngine;
using UnityEngine.UI;
using Util;


public class TutorialSlideNavigation : MonoBehaviour
{
	 //assign the button here
	 public Button pressAFor;
	 public Button pressBFor;
	 public Button pressXFor;
	 public Button pressYFor;

    // Update is called once per frame
    void Update()
    {
    	if (Controller.GetA())
    	{
    		pressAFor.Select();
    		pressAFor.onClick.Invoke();
    	}

    	if (Controller.GetB())
    	{
    		pressBFor.Select();
    		pressBFor.onClick.Invoke();

    	}

    	if (Controller.GetX())
    	{
    		pressXFor.Select();
    		pressXFor.onClick.Invoke();

    	}

    	if (Controller.GetY())
    	{
    		pressYFor.Select();
    		pressYFor.onClick.Invoke();

    	}

    }
}
