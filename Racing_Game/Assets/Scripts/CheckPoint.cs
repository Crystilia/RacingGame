using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	public GameObject LapFinish;
	public GameObject HalfLapTrig;

	void OnTriggerEnter () {
		LapFinish.SetActive (true);
		HalfLapTrig.SetActive (false);
	}
}
