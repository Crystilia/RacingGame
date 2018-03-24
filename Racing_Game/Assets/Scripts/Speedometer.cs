using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour {

    static float minAngle = 290.0f;
    static float maxAngle = 90.0f;
    static Speedometer Speeder;
    public GameObject car;
	
	void Start () {
		Speeder = this;
        car = GameObject.FindGameObjectWithTag("Vehicle");
	}

    private void Update()
    {
        ShowSpeed(car.GetComponent<Rigidbody>().velocity.magnitude, 0f, car.GetComponent<Test_newTurn>().terminalVelocity);
    }

    public static void ShowSpeed(float speed, float min, float max)
	{
		float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(min, max, speed));
		Speeder.transform.eulerAngles = new Vector3(0,0,ang);
	}
}
