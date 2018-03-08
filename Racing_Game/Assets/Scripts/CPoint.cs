using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPoint : MonoBehaviour {

	public GameObject LapFinish;
	public GameObject CheckLapTrig;

	void OnTriggerEnter () {
		LapFinish.SetActive (true);
		CheckLapTrig.SetActive (false);
	}
}
