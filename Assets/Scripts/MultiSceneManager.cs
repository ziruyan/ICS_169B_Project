using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiSceneManager
{
	public static float GameTime;
	public static int winOrLose;

	public static void End_Main_Scene(float time, int winLose)
	{
		GameTime = time;
		winOrLose = winLose;
	}

	public static float get_time()
	{
		return GameTime;
	}

	public static int get_Win()
	{
		return winOrLose;
	}
}
