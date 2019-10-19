using UnityEngine;
using UnityEngine.UI;

public class ToolTextDisplay : MonoBehaviour
{
    public Text toolDisplay;
    public Text scoreDisplay;
    public RaccoonController raccoonController;

    public ToolController tool;
    private string tType;


    void Update()
    {
        // Display Tool Text
        // tool = ToolController.toolInHand;
        // if (tool != null)
        // {
        //     tType = tool.toolType;
        //     toolDisplay.text = "Tool: " + tType;
        // }
        // else
        // {
        //     toolDisplay.text = "Tool: None";
        // }
        // // Display Score Text
        // scoreDisplay.text = "Score: " + RaccoonController.score;

    }

}
