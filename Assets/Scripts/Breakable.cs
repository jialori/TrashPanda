using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Note: 
	Any GameObject using the Breakable component should also have a RigidBody component.
*/
public class Breakable : MonoBehaviour
{

	public void trigger() 
	{
		Destroy(this.gameObject);
	}

}
