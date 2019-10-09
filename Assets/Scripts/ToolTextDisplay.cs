using UnityEngine;
using UnityEngine.UI;

public class ToolTextDisplay : MonoBehaviour
{
    public Text toolDisplay;

    private ToolController tool;
    private string tType;


    void Update()
    {
        tool = ToolController.toolInHand;
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
