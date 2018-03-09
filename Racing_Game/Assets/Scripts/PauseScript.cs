using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    private GameObject pauseMenuUI;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        pauseMenuUI = GameObject.Find("Pause Menu Panal");
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeBtn();
            }
            else
            {
                Pause();
            }
        }
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
        paused = false;
    }

    public void ResumeBtn()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
}
