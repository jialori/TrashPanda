using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public bool failureCondition = false;       // Variable that will determine whether the player has lost or not
    public string sceneName;                    // Name of the new scene to transition to

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))        // Condition for scene transition here. Right now it triggers on pressing 'P' for testing purposes
        {
            Debug.Log("P down");
            AudioManager.instance.Play("GameOver");
            SceneManager.LoadScene(sceneName);
        }
    }
}
