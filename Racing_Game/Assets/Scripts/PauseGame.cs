using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {
	public Transform pauseMenu;
    public Button resume;


	void Start (){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
		{
			Pause();
			
		}
	}

	public void ExitGameButton()
	{
		Application.Quit();
	}

	public void mainMenu(string MainMenu)
	{
		SceneManager.LoadScene("Main Menu");
	}
	public void Pause()
	{
		resume.Select();
		if (pauseMenu.gameObject.activeInHierarchy == false)
		{
			pauseMenu.gameObject.SetActive(true);
			Time.timeScale = 0;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = (true);

		}
		else
		{
			pauseMenu.gameObject.SetActive(false);
			Time.timeScale = 1;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = (false);

		}
		
	}

}