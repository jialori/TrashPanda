using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectiveFinalShowcase : MonoBehaviour
{
	public List<TextMeshProUGUI> objectiveDisplays;

    void Start()
    {
    	StartCoroutine("AutoScrollObjectiveList");
    }


    IEnumerator AutoScrollObjectiveList()
    {
        List<GameTask> completedTasks = TaskManager.instance.GetCompletedTasks();
        // Debug.Log("total count:" + completedTasks.Count);
        int count = Mathf.Min(completedTasks.Count, 3);

        for (int i=0; i < count; i++) 
        {
	        objectiveDisplays[i].text = completedTasks[i].description;
	        yield return new WaitForSeconds(.3f);
        }

        if (count < 3)
        {
        	yield break;
        }

        for (int i=0; i < completedTasks.Count - count; i++) 
        {
        	for (int j = 0; j < 3; j++)
        	{
		        // Debug.Log("current displaying as first :" + (i + 1 + j + 1));
		        objectiveDisplays[j].text = completedTasks[i + 1 + j].description;
        	}
	        yield return new WaitForSeconds(.3f);        		
        }

    }
}
