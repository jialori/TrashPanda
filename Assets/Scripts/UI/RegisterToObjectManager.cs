using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ObjectManager.instance.RegisterHealthBarObj(this.gameObject);
    }
}
