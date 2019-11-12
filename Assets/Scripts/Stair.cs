using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private int floor;

    private Vector3 position = new Vector3(29, 1.6f, -16.5f);
    // Start is called before the first frame update
    void Start()
    {
        position += (floor - 1) * new Vector3(0, 8, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetFloor () 
    {
        return this.floor;
    }

    public Vector3 GetPosition() 
    {
        return position;
    }

    public void OpenDoor()
    {

    }

    public void CloseDoor()
    {
        
    }
}
