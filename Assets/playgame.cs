using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
	public static Mainmenu instance;

	void Awake()
    {
        instance = this;
    }

	void Start ()
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
}
