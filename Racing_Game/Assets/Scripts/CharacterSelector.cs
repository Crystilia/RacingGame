using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CharacterSelector : MonoBehaviour {

    public GameObject[] vehicleList;
    private Animator animator;
    private new Camera camera;
    private int index = 0;
    Vector3 targetDir;
    Vector3 theta;

    // Use this for initialization
    void Start () {
        vehicleList = new GameObject[transform.childCount];
        camera = GameObject.Find("VehicleSelectCam").GetComponent<Camera>();
        animator = GameObject.Find("Character1").GetComponent<Animator>();
        for (int i = 0; i < transform.childCount; i++)
        {
            vehicleList[i] = transform.GetChild(i).gameObject;
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Testing Zone");
    }

    public void Confirm()
    {
        animator.SetTrigger("Selected");
        PlayerPrefs.SetInt("VehicleSelected", index);
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
