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