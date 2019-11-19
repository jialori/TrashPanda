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
        Debug.Log(completedTasks.Count);
        int length = Mathf.Min(completedTasks.Count, 3);
        for (int i=0; i < length; i++) 
        {
        	Debug.Log(i);
	        objectiveDisplays[i].text = completedTasks[i].description;
        }
        yield return new WaitForSeconds(3);
    }
}
