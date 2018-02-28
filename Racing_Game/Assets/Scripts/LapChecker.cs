using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapChecker : MonoBehaviour {
private int count;
public Text Lapcount;
public GameObject CheckpointTrigger;
public GameObject laptrigger;


	void Start (){
		count = 1;
		Lapcount.text = "Lap: " + count.ToString();
	}


	

	void OnTriggerEnter(Collider other)
	{
	if (other.gameObject.CompareTag("LapTrigger"))

	{
	count = count + 1;
	Lapcount.text = "Lap: " + count.ToString();
	}

	}
}
