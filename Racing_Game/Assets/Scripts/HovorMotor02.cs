using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovorMotor02 : MonoBehaviour {
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
    }
	
	// Update is called once per frame
	void Update () {
        thrustInput = Input.GetAxis("Vertical") * thrust;
        turnInput = Input.GetAxis("Horizontal") *turnSpeed;
        Hover();
    }

    private void Hover()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (transform.rotation.x > 0)
        {
            transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        }

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
    }

}
