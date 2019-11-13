using UnityEngine;

namespace Util
{
	class Util
	{
		public static string GetFormattedTime(float time)
		{
			var mins = Mathf.Floor(time / 60);
	        var secs = time % 60;
			return string.Format("{0:00} min {1:00} sec", mins, secs);
		}
	}

	class Controller 
	{
		public static bool GetA()
		{
		    if (GameManager.instance.UseController)
		    {
		        return Input.GetButtonDown("A");
		    }
		    else
		    {
		        return Input.GetKeyDown(KeyCode.Space);
		    }
		}

	    public static bool GetB()
	    {
	        if (GameManager.instance.UseController)
	        {
	            return Input.GetButtonDown("B");
	        }
	        else
	        {
	            return Input.GetKeyDown("e");
	        }
	    }

        public static bool GetX()
	    {
	        if (GameManager.instance.UseController)
	        {
	            return Input.GetButtonDown("X");
	        }
	        else
	        {
	            return Input.GetKeyDown("x");
	        }
	    }

	    public static bool GetY()
	    {
	        if (GameManager.instance.UseController)
	        {
	            return Input.GetButtonDown("Y");
	        }
	        else
	        {
	            return Input.GetKeyDown("y");
	        }
	    }

		public static bool GetPause()
		{
		    if (GameManager.instance.UseController)
		    {
		        return Input.GetButtonDown("Pause");
		    }
		    else
		    {
		        return Input.GetKeyDown("p");
		    }
		}


		public static float GetXAxis()
		{
		    if (GameManager.instance.UseController)
		    {
		        return Input.GetAxis("LeftJoystickX");
		    }
		    else
		    {
		        return Input.GetAxis("Horizontal");
		    }
		}

		public static float GetYAxis()
		{
		    if (GameManager.instance.UseController)
		    {
		        return -Input.GetAxis("LeftJoystickY");
		    }
		    else
		    {
		        return Input.GetAxis("Vertical");
		    }
		}

		public static float GetCamXAxis()
		{
		    if (GameManager.instance.UseController)
		    {
		        return Input.GetAxis("RightJoystickX");
		    }
		    else
		    {
		        return Input.GetAxis("Mouse X");
		    }
		}

	    public static float GetCamYAxis()
	    {
	        if (GameManager.instance.UseController)
	            return Input.GetAxis("RightJoystickY");
	        else
	            return Input.GetAxis("Mouse Y");
	    }
	}

	class MyLayers {
	    static private string breakableMaskName = "Breakable";
	    static private string knockableMaskName = "Knockable";
	    static private string toolsMaskName = "Tools";
	    static private string interactableMaskName = "Interactable";
	    
        static public int breakableMask = 1 << LayerMask.NameToLayer(breakableMaskName);
        static public int knockableMask = 1 << LayerMask.NameToLayer(knockableMaskName);
        static public int toolsMask = 1 << LayerMask.NameToLayer(toolsMaskName);
        static public int interactableMask = 1 << LayerMask.NameToLayer(interactableMaskName);
	}

}