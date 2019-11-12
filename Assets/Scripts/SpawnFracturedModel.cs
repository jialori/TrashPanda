using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFracturedModel : MonoBehaviour
{
    public GameObject orgObj;
    public GameObject fracturedObj;
    public float mass = 0.001f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.5)
            SpawnFracturedObj(collision.relativeVelocity * mass);
    }

    public void SpawnFracturedObj(Vector3 momentum)
    {
        Vector3 orgPos = transform.position;
        Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
        Quaternion orgQua = transform.rotation;
        Debug.Log(transform.localScale);
        Destroy(orgObj);
        Debug.Log(orgPos);
        GameObject newObj = Instantiate(fracturedObj, orgPos, orgQua) as GameObject;
        newObj.transform.localScale = scale;
        newObj.GetComponent<ExplodeScript>().Explode(momentum);
    }
}
