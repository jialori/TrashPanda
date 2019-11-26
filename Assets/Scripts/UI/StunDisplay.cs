using UnityEngine;
using TMPro;
using System;

public class StunDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stunText;
    [SerializeField] RaccoonController raccoon;

    void Start()
    {
        // Commented this out because it was causing the stun display text to not appear when stunned
        //raccoon = GameManager.instance.Raccoon;
    }

    void Update()
    {
        // TODO: inefficient, use coroutine instead
        if ((raccoon ? raccoon.isStunned : false))
        {
            stunText.enabled = true;
        }
        else
        {
            //Debug.Log("raccoon: " + Convert.ToBoolean(raccoon).ToString() + ", raccoon.isStunned: " + raccoon.isStunned.ToString());
            stunText.enabled = false;
        }
    }
}
