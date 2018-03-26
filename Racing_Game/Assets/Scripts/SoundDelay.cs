using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDelay : MonoBehaviour {

    public AudioClip startSound;
<<<<<<< HEAD
    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        Invoke("StartTimer", 1.5f);
	}
=======
    public AudioClip musicStart;
    private AudioSource source;
    public GameObject LapTimer;

    // Use this for initialization
    void Start() {
        source = GetComponent<AudioSource>();
        Invoke("StartTimer", 1.5f);
        Invoke("EndTimer", 4.0f);
        InvokeRepeating("PlayMyMusic", 4.0f, 0);
    }
>>>>>>> e9a129566e04d7125c64ed89333891677966ff3d

    void StartTimer()
    {

        StartSound();
<<<<<<< HEAD
    }
	
    public void StartSound()
    {
        source.PlayOneShot(startSound, 1.0f);
    }

=======

    }

    public void StartSound()
    {
        source.PlayOneShot(startSound, 1.0f);

    }

    public void EndTimer()
    {
        LapTimer.SetActive(true);
        
        
    }

    public void PlayMyMusic ()
    {
        MusicStart();
    }

    public void MusicStart()
    {
        source.PlayOneShot(musicStart);
    }
>>>>>>> e9a129566e04d7125c64ed89333891677966ff3d
}
