using UnityEngine;
using TMPro;

public class StunDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stunText;
    [SerializeField] RaccoonController raccoon;

    void Start()
    {
        //raccoon = GameObject.Find("Raccoon").GetComponent<RaccoonController>();
    }

    void Update()
    {
        if (raccoon.isStunned)
        {
            stunText.enabled = true;
        }
        else
        {
            stunText.enabled = false;
        }
    }
}
