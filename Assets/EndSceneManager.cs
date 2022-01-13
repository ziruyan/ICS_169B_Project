using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

	public string start_scene_name;

    void Start()
    {
        this.enabled = true;
    }

    // Update is called once per frame

	public void GoToStartScene()
	{
		SceneManager.LoadScene("Main");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}