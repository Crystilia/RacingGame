using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapChecker : MonoBehaviour {
	
	public Text Lapcount;
	public GameObject CheckpointTrigger;
	public GameObject laptrigger;

	private int count;
	private bool checkpoint;


	void Start (){
		count = 1;
		checkpoint = false;
		Lapcount.text = "Lap: " + count.ToString();
	}


	

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == ("LapTrigger") && checkpoint == true) {
			count = count + 1;
			Lapcount.text = "Lap: " + count.ToString ();
			checkpoint = false;
		} else if (other.tag == ("checkTrigger")) {
			checkpoint = true;
		}

	}
}
