using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotor01 : MonoBehaviour {
    public float speed;
    private float speedHolder;
    public float turnSpeed;
    public float thrust;
    public float hoverHeight;

    private Quaternion targetRotation;
    private float thrustInput;
    private float turnInput;
    private Rigidbody crb;

	// Use this for initialization
	void Start () {
        crb = GetComponent<Rigidbody>();
        crb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void Update () {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
	}

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * thrust;
            crb.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        else
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = -Vector3.up * proportionalHeight * (thrust);
            crb.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        crb.AddForce(0f, 0f, thrustInput * speed);
        crb.AddTorque(0f, turnInput * turnSpeed, 0f);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Jump")
        {
            print("constrain");
            //transform.rotation= Quaternion.Lerp (transform.rotation, targetRotation ,Time.deltaTime);
            //StartCoroutine(Stabalize());
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.time*0.01);
            //transform.rotation = Quaternion.identity;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.name.Contains("Pickup"))
        {
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.name == "BoostJump")
        {
            StartCoroutine(SpeedBoost());
            print("boost");
        }
        else if (other.gameObject.name == "Jump")
        {
            print("no constraints");
            crb.constraints = RigidbodyConstraints.None;
        }
    }

    IEnumerator Stabalize()
    {
        yield return new WaitForSeconds(1f);
        crb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        yield return null;
    }

    IEnumerator SpeedBoost()
    {
        //increase speed
        for(int i = 10; i >=0; i--)
        {
            speed += speed/2;
            yield return new WaitForSeconds(0.1f);
        }
        
        //decrease speed
        for(int i = 10; i >= 0; i--)
        {
            speed -= speed/2;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
