using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapChecker : MonoBehaviour {
private int count;
public Text Lapcount;


	void Start (){
		count = 1;
		Lapcount.text = "Lap: " + count.ToString();
	}
	void Update(){
		
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
