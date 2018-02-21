using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour {

static float minAngle = 290.0f;
static float maxAngle = 90.0f;
static Speedometer Speeder;
	
	void Start () {
		Speeder = this;
	}
	
	
	public static void ShowSpeed(float speed, float min, float max)
	{
		float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(min, max, speed));
		Speeder.transform.eulerAngles = new Vector3(0,0,ang);
	}
}
