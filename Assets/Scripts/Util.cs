using UnityEngine;

namespace Util
{
	class Controller {

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


		public static bool GetPause()
		{
		    if (GameManager.instance.UseController)
		    {
		        return Input.GetButtonDown("Pause");
		    }
		    else
		    {
		        return Input.GetKeyDown(KeyCode.Escape);
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

}