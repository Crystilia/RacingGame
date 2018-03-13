using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CharacterSelector : MonoBehaviour {

    public GameObject[] vehicleList;
    public Animator[] animators;
    public bool male = true;
    private new Camera camera;
    private int index = 0;
    bool selected = true;
    Vector3 targetDir;
    Vector3 theta;

    // Use this for initialization
    void Start () {
        camera = GameObject.Find("VehicleSelectCam").GetComponent<Camera>();
        animators = new Animator[GameObject.Find("CharacterList").transform.childCount];
        vehicleList = new GameObject[transform.childCount];

        //populate vehicleList
        for (int i = 0; i < transform.childCount; i++)
        {
            vehicleList[i] = transform.GetChild(i).gameObject;
            vehicleList[i].transform.GetChild(0).GetComponent<Animator>().gameObject.SetActive(false);
            vehicleList[i].transform.GetChild(1).GetComponent<Animator>().gameObject.SetActive(true);
        }

        //foreach (GameObject g in vehicleList)
        //{
        //    vehicleList[index].transform.GetChild(0).GetComponent<Animator>().gameObject.SetActive(false);
        //    vehicleList[index].transform.GetChild(0).GetComponent<Animator>().gameObject.SetActive(false);
        //}
        //populate animators and set female to not active
        //for (int i = 0; i < GameObject.Find("CharacterList").transform.childCount; i++)
        //{
        //    animators[i] = GameObject.Find("CharacterList").transform.GetChild(i).GetComponent<Animator>();
        //    if (i % 2 == 0)
        //    {
        //        animators[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        animators[i].gameObject.SetActive(true);
        //    }
        //}
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("Testing Zone");
    }

    public void PlaySelectAnim()
    {
        if (male)
        {
            vehicleList[index].transform.GetChild(1).GetComponent<Animator>().SetTrigger("Selected");
            //animators[index + 1].SetTrigger("Selected");
        }
        else
        {
            vehicleList[index].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Selected");
        }
        //switch (index)
        //{
        //    case 0:
        //        if (male)
        //        {
        //            animators[1].SetTrigger("Selected");
        //        }
        //        else
        //        {
        //            animators[0].SetTrigger("Selected");
        //        }
        //        break;
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //}
    }

    public void SelectGender()
    {
        if (!male)
        {
            male = !male;
            for (int i = 0; i < transform.childCount; i++)
            {
                male = !male;
                vehicleList[i].transform.GetChild(0).GetComponent<Animator>().gameObject.SetActive(false);
                vehicleList[i].transform.GetChild(1).GetComponent<Animator>().gameObject.SetActive(true);
            }
        }
        else
        {
            male = !male;
            for (int i = 0; i < transform.childCount; i++)
            {
                male = !male;
                vehicleList[i].transform.GetChild(0).GetComponent<Animator>().gameObject.SetActive(true);
                vehicleList[i].transform.GetChild(1).GetComponent<Animator>().gameObject.SetActive(false);
            }
        }
        
        //if (male)
        //{
        //    male = !male;
            //    for (int i = 0; i < GameObject.Find("CharacterList").transform.childCount; i++)
            //    {
            //        animators[i] = GameObject.Find("CharacterList").transform.GetChild(i).GetComponent<Animator>();
            //        if (i % 2 == 0)
            //        {
            //            animators[i].gameObject.SetActive(true);
            //        }
            //        else
            //        {
            //            animators[i].gameObject.SetActive(false);
            //        }
            //    }
            //}
            //else
            //{
            //    male = !male;
            //    for (int i = 0; i < GameObject.Find("CharacterList").transform.childCount; i++)
            //    {
            //        animators[i] = GameObject.Find("CharacterList").transform.GetChild(i).GetComponent<Animator>();
            //        if (i % 2 == 0)
            //        {
            //            animators[i].gameObject.SetActive(false);
            //        }
            //        else
            //        {
            //            animators[i].gameObject.SetActive(true);
            //        }
            //    }
            //}
        }

    public void Confirm()
    {
        PlayerPrefs.SetInt("VehicleSelected", index);
        PlaySelectAnim();
        StartCoroutine(LoadScene());
    }

    public void LeftArrow()
    {
        index--;

        if (index < 0)
        {
            index = vehicleList.Length - 1;
        }
    }

    public void RightArrow()
    {
        index++;

        if (index >= transform.childCount)
        {
            index = 0;
        }
    }

    private void Update()
    {
        targetDir = vehicleList[index].GetComponent<Transform>().position - camera.transform.position;
        theta = Vector3.RotateTowards(camera.transform.forward, targetDir, Time.deltaTime * 2.5f, 0f);
        camera.transform.rotation = Quaternion.LookRotation(theta);
    }
}
