using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDelay : MonoBehaviour {

    public AudioClip startSound;
    public AudioClip musicStart;
    private AudioSource source;
    public GameObject LapTimer;
    public GameObject StartEngines;

    // Use this for initialization
    void Start() {
        source = GetComponent<AudioSource>();
        Invoke("StartTimer", 1.5f);
        Invoke("EndTimer", 4.0f);
        InvokeRepeating("PlayMyMusic", 4.0f, 0);
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
        StartEngines.SetActive(false);
        
    }

    public void PlayMyMusic ()
    {
        MusicStart();
    }

    public void MusicStart()
    {
        source.PlayOneShot(musicStart);
    }
}
