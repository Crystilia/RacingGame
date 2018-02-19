using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//slider to show the loading progress
//after finish loading -> take to sceneName that was preset as chara

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    private string sceneName = "Chara";

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }

    IEnumerator Loading(string loadScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene);
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            if (operation.progress == 0.9f)
            {
                Debug.Log("Press any key to start");
                if (Input.anyKeyDown)
                    operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
