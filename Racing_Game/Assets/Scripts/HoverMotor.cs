using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotor : MonoBehaviour {
	public float speed = 350f;
	public float turnSpeed = 5f;
	public float hoverForce = 65f;
	public float hoverHeight = 3f;

	private float powerInput;
	private float turnInput;
	private Rigidbody carRigidbody;
	// Use this for initialization

	void Awake () {
		carRigidbody = GetComponent <Rigidbody> ();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		powerInput = Input.GetAxis ("Vertical");
		turnInput = Input.GetAxis ("Horizontal");
	}

	void FixedUpdate() {
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, hoverHeight))
		{
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
			carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
        else
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = -Vector3.up * proportionalHeight * (hoverForce-10);
			carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            //Debug.Log("flying?");
        }
			carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
			carRigidbody.AddTorque(0f, turnInput *turnSpeed, 0f);
	}
}
