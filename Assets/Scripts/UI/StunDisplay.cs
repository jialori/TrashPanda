using UnityEngine;
using TMPro;

public class StunDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stunText;
    [SerializeField] RaccoonController raccoon;

    void Start()
    {
        raccoon = GameManager.instance.Raccoon;
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
            stunText.enabled = false;
        }
    }
}
