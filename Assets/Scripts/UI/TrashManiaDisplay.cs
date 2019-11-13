using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashManiaDisplay : MonoBehaviour
{
    private Tool enabledTool; 
    [SerializeField] private TextMeshProUGUI timeLeft;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            Debug.Log(enabledTool);
            timeLeft.text = Util.Util.GetFormattedTime(enabledTool.GetTimer());
        }
    }

    public void Enable(Tool tool)
    {
        enabledTool = tool; 
    }

    public void Disable()
    {
        enabledTool = null;
    }
}
