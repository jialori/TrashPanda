using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController2 : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;

    public float speed;
    public float allowPlayerRotation;
}