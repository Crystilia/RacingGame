using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapChecker : MonoBehaviour
{

    public Text Lapcount;
    public Text winTxt;
    private int count;

    void Start()
    {
        count = 0;
        Lapcount.text = "Lap: " + count.ToString();
        winTxt.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            count++;
            Lapcount.text = "Lap: " + count.ToString();

            if(count == 3)
            {
                winTxt.enabled = true;
            }
        }
    }
}

