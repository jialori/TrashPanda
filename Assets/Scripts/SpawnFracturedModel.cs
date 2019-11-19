using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFracturedModel : MonoBehaviour
{
  public GameObject orgObj;
  public GameObject fracturedObj;
  //public GameObject raccoon;
  public float mass = 0.001f;
  private DestroyEffect df;
  //private bool fragile = false;

  private void Start()
  {
    df = GetComponent<DestroyEffect>();
  }
  void OnCollisionEnter(Collision collision)
  {
    //if (collision.gameObject == raccoon) fragile = true;
    if (ObjectManager.curTool != null)
    {
      df.StartDusting(true);
      SpawnFracturedObj(collision.relativeVelocity * mass);
    }
  }

  public void SpawnFracturedObj(Vector3 momentum)
  {
    Vector3 orgPos = transform.position;
    // Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
    Quaternion orgQua = transform.rotation;
    Debug.Log("Object " + orgObj + " destroyed.");
    Destroy(orgObj);
    GameObject newObj = Instantiate(fracturedObj, orgPos, orgQua) as GameObject;
    // newObj.transform.localScale = scale;
    newObj.GetComponent<ExplodeScript>().Explode(momentum);
  }
}