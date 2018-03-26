using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDelay : MonoBehaviour {

    public AudioClip startSound;
    private AudioSource source;
    public GameObject LapTimer;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        Invoke("StartTimer", 1.5f);
        Invoke("EndTimer", 4.0f);
	}

    void StartTimer()
    {
        
        StartSound();

    }
	
    public void StartSound()
    {
        source.PlayOneShot(startSound, 1.0f);
        
    }

    public void EndTimer()
    {
        LapTimer.SetActive(true);
    }

}
