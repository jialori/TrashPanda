using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour
{
    public Text toolDisplay;

    public ToolController tool;
    private string tType;


    void Update()
    {
        // Display Tool Text;
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
