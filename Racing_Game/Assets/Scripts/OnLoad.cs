using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoad : MonoBehaviour {
    public GameObject[] vehicleList;
    private RacingAI ai;
    private MLP_HoverMotor motor;
    public FollowCam followcam;
    public Speedometer speedometer;

    // Use this for initialization
    void Start () {
        followcam = GameObject.Find("FollowCam").GetComponent<FollowCam>();
        vehicleList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            vehicleList[i] = transform.GetChild(i).gameObject;
        }

        switch (PlayerPrefs.GetInt("VehicleSelected"))
        {
            case 1:
                print("1");
                foreach (GameObject g in vehicleList)
                {
                    if (!g.name.Equals("truckFinal"))
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = true;
                        motor.enabled = false;
                        //g.SetActive(false);
                    }
                    else
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = false;
                        motor.enabled = true;
                        followcam.target = g.transform;
                        speedometer.car = g;
                    }
                }
                break;
            case 2:
                print("2");
                foreach (GameObject g in vehicleList)
                {
                    if (!g.name.Equals("sportFinal"))
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = true;
                        motor.enabled = false;
                        //g.SetActive(false);
                    }
                    else
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = false;
                        motor.enabled = true;
                        followcam.target = g.transform;
                        speedometer.car = g;
                    }
                }
                break;
            case 3:
                print("3");
                foreach(GameObject g in vehicleList)
                {
                    if (!g.name.Equals("limoFinal"))
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = true;
                        motor.enabled = false;
                        //g.SetActive(false);
                    }
                    else
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = false;
                        motor.enabled = true;
                        followcam.target = g.transform;
                        speedometer.car = g;
                    }
                }
                break;
            case 0:
                print("4");
                foreach (GameObject g in vehicleList)
                {
                    if (!g.name.Equals("muscleFinal"))
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = true;
                        motor.enabled = false;
                        //g.SetActive(false);
                    }
                    else
                    {
                        ai = g.GetComponent<RacingAI>();
                        motor = g.GetComponent<MLP_HoverMotor>();
                        ai.enabled = false;
                        motor.enabled = true;
                        followcam.target = g.transform;
                        speedometer.car = g;
                    }
                }
                break;
        }
    }
}
