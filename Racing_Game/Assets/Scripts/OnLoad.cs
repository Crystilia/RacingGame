using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoad : MonoBehaviour {
    public GameObject[] vehicleList;
    public FollowCam followcam;

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
                    if (!g.name.Equals("truckV0.0.03"))
                    {
                        g.SetActive(false);
                    }
                    else
                    {
                        followcam.target = g.transform;
                    }
                }
                break;
            case 2:
                print("2");
                foreach (GameObject g in vehicleList)
                {
                    if (!g.name.Equals("sportsV0.0.02"))
                    {
                        g.SetActive(false);
                    }
                    else
                    {
                        followcam.target = g.transform;
                    }

                }
                break;
            case 3:
                print("3");
                foreach(GameObject g in vehicleList)
                {
                    if (!g.name.Equals("limoV0.0.01"))
                    {
                        g.SetActive(false);
                    }
                    else
                    {
                        followcam.target = g.transform;
                    }
                }
                break;
            case 0:
                print("4");
                foreach (GameObject g in vehicleList)
                {
                    if (!g.name.Equals("muscleV0.0.02"))
                    {
                        g.SetActive(false);
                    }
                    else
                    {
                        followcam.target = g.transform;
                    }
                }
                break;
        }
    }
}
