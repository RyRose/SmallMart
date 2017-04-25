using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scene : MonoBehaviour {

	public void LoadCredits()
	{
		SceneManager.LoadScene("_Credits");
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("_MainMenu");
	}

	public void LoadGame()
	{
		SceneManager.LoadScene("_HomeIntro");
	}

}