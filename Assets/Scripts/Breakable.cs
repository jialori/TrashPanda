using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Note: 
	Any GameObject using the Breakable component should also be assigned a Layer "Breakable".
	It is NOT using RigidBody for collision detection. 
*/
public class Breakable : MonoBehaviour
{

	public void trigger() 
	{
		Destroy(this.gameObject);
	}

}
