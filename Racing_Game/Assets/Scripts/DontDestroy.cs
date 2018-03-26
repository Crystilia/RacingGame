using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            DontDestroyOnLoad(this.gameObject);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
        {
            Destroy(this.gameObject);
        }
    }
}
