using UnityEngine;
using UnityEngine.UI;

public class ToolDisplay : MonoBehaviour
{
    public Text toolDisplay;

    public Tool tool;
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
