using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour {

	public static int MinCount;
	public static int SecCount;
	public static float MilliCount;
	public static string MilliDisplay;

	public GameObject MinBox;
	public GameObject SecBox;
	public GameObject MilliBox;
	

	void Update () {
		MilliCount += Time.deltaTime * 10;
		MilliDisplay = MilliCount.ToString ("F0");
		MilliBox.GetComponent<Text> ().text = "" + MilliDisplay;

		if (MilliCount >= 10) {
			MilliCount = 0;
			SecCount += 1;
		}

		if (SecCount <= 9) {
			SecBox.GetComponent<Text> ().text = "0" + SecCount + ".";
		} else {
			SecBox.GetComponent<Text> ().text = "" + SecCount + ".";
		}

		if (SecCount >= 60) {
			SecCount = 0;
			MinCount += 1;
		}

		if (MinCount <= 9) {
			MinBox.GetComponent<Text> ().text = "0" + MinCount + ":";
		} else {
			MinBox.GetComponent<Text> ().text = "" + MinCount + ":";
		}
		
	}
}