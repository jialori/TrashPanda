using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toolDisplay;

    private Tool tool;
    private string tType;


    void Update()
    {
        // Display Tool Text;
        tool = ObjectManager.curTool;
        if (tool != null)
        {
            tType = tool.toolType;
            toolDisplay.text = "Tool: " + tType;
        }
        else
        {
            toolDisplay.text = "Tool: None";
        }

    }

}
