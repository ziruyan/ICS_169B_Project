using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{

    public GameObject settings_image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void playgame()
    {
        SceneManager.LoadScene("Main",LoadSceneMode.Single);
    }

	public void quitgame()
    {
        Application.Quit();
    }

    	public void ShowSettings()
    {
		if (settings_image.activeSelf == false)
		{
			settings_image.SetActive(true);
		}
		else
		{
			settings_image.SetActive(false);
		}
	}

	public void CloseSettings()
	{
		settings_image.SetActive(false);
	}
	
}
