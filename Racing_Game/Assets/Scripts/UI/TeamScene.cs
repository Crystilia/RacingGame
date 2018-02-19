using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//team logo show for 'timer' seconds -> changable
//skip button show after 'timer' seconds -> take to loadingscene

public class TeamScene : MonoBehaviour
{
    public Button skipButton;

    private string loadScene = "Loading";

    private float timer = 15f;

    void Start()
    {
        skipButton = GetComponent<Button>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            skipButton.enabled = true;
        }
    }

    public void SkipButton()
    {
        SceneManager.LoadScene(loadScene);
    }
}