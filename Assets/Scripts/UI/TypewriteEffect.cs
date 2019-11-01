using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriteEffect : MonoBehaviour
{
    [Header("Effect Settings")]
	public Text displayTxt;
	[TextArea] public string fullText;
	public float deltaDelay;

	private string _curText;
	private float _randRange;
	private float _deltaDelayRand;

    void Start()
    {
    	displayTxt = GetComponent<Text>();
        StartCoroutine(ShowText());
        _randRange = deltaDelay * 0.1f;
    }

    IEnumerator ShowText()
    {
    	for (int i = 0; i < fullText.Length; i++)
    	{
    		_curText = fullText.Substring(0, i);
    		displayTxt.text = _curText;
    		_deltaDelayRand = deltaDelay + Random.Range(0, _randRange);
    		yield return new WaitForSeconds(_deltaDelayRand);
    	}
    } 

}
