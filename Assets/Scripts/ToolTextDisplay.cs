using UnityEngine;
using UnityEngine.UI;

public class ToolTextDisplay : MonoBehaviour
{
    public Text toolDisplay;

    private ToolController tool;
    private string tType;

    void Start()
    {
        tool = FindObjectOfType<ToolController>();
    }

    void Update()
    {
        tType = tool.toolType;
        if (tool.beingCarried)
        {
            toolDisplay.text = "Tool: " + tType;
        }
        else
        {
            toolDisplay.text = "Tool: None";
        }
        
    }

}
