using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Animator camera;

    public GameObject tournamentButton;
    public GameObject optionButton;
    public GameObject quitButton;

    public GameObject numPlayerButton;
    public GameObject genderButton;
    public GameObject vehicleButton;

    public GameObject graphicButton;
    public GameObject volumeButton;
    public GameObject loadButton;

    public GameObject backButton;
    public GameObject startButton;

    private void Start()
    {
        tournamentButton.SetActive(true);
        optionButton.SetActive(true);
        quitButton.SetActive(true);

        numPlayerButton.SetActive(false);
        genderButton.SetActive(false);
        vehicleButton.SetActive(false);

        graphicButton.SetActive(false);
        volumeButton.SetActive(false);
        loadButton.SetActive(false);

        backButton.SetActive(false);
        startButton.SetActive(false);
    }

    //click tournament button
    public void TBonClick()
    {
        numPlayerButton.SetActive(true);
        genderButton.SetActive(true);
        vehicleButton.SetActive(true);

        backButton.SetActive(true);
        startButton.SetActive(true);
    }

    //click option button
    public void OBonClick()
    {
        graphicButton.SetActive(true);
        volumeButton.SetActive(true);
        loadButton.SetActive(true);

        backButton.SetActive(true);
    }

    //click back button
    public void BBonClick()
    {
        numPlayerButton.SetActive(false);
        genderButton.SetActive(false);
        vehicleButton.SetActive(false);

        graphicButton.SetActive(false);
        volumeButton.SetActive(false);
        loadButton.SetActive(false);

        backButton.SetActive(false);
        startButton.SetActive(false);
    }

    public void TBCameraSwitch()
    {
        TBonClick();
        camera.SetBool("OnClick", true);
    }

    public void OBCameraSwitch()
    {
        OBonClick();
        camera.SetBool("OnClick", true);
    }

    public void CameraSwitchBack()
    {
        BBonClick();
        camera.SetBool("OnClick", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ProtoType");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //start button will only become available if the player select numplayer, gender and vehicle
    //haven't write the code yet <- no idea how you guys plan on dealing with this
    //will just left the start button enable so you can switch to the game scene for now
}
