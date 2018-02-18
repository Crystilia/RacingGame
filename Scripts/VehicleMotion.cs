using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMotion : MonoBehaviour {

    public float speed;

    //Drive vars
    public float thrust = 17f;
    public float VelocitySlowingFactor = 0.99f;
    public float velocityBrakingFactor = 0.95f;
    public float angleOfRoll = 30f;

    //Hover vars
    public float hoverHeight = 1.5f;
    public float maxHoverHeight = 5f;
    public float hoverForce = 300f;
    public LayerMask groundMask;
    public MyPID pid;

    //Physics vars
    public Transform vehicleBody;
    public float terminalVelocity = 100f;
    public float hoverGrav = 20f;
    public float fallGrav = 80f;

    Rigidbody rb;
    PlayerInput input;
    float drag;
    bool grounded;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        drag = thrust / terminalVelocity;
	}
	
	// Update is called once per frame
	void Update () {
        speed = Vector3.Dot(rb.velocity, transform.forward);
        CalcHover();
        CalcThrust();
	}

    void CalcHover()
    {
        Vector3 groundNormal;

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        grounded = Physics.Raycast(ray, out hit, maxHoverHeight, groundMask);

        if (grounded)
        {
            float height = hit.distance;
            groundNormal = hit.normal.normalized;
            float forcePercent = pid.Update((hoverHeight-height), Time.deltaTime);
            Vector3 force = groundNormal * hoverHeight * forcePercent;
            Vector3 gravity = -groundNormal * hoverGrav * height;

            rb.AddForce(force, ForceMode.Acceleration);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
        else
        {
            groundNormal = Vector3.up;
            Vector3 gravity = -groundNormal * fallGrav;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));
    }

    void CalcThrust()
    {
        float rotationTorque = input.TurnInput - rb.angularVelocity.y;
        rb.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);
        float sideSpeed = Vector3.Dot(rb.velocity, transform.right);
        Vector3 sideForce = transform.right * (sideSpeed / Time.deltaTime);
        //rb.AddForce(sideForce, ForceMode.Acceleration);

        if (input.thrustInput <= 0f)
            rb.velocity *= VelocitySlowingFactor;
        if (!grounded)
            return;
        if (input.isBraking)
        {
            rb.velocity *= velocityBrakingFactor;
        }

        float propulsion = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            if (input.TurnInput > 0)
            {
                propulsion = ((thrust * input.thrustInput) + (thrust * input.TurnInput)) - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
                rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
            }
            else if (input.TurnInput < 0)
            {
                propulsion = ((thrust * input.thrustInput) - (thrust * input.TurnInput)) - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
                rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
            }
        }
        propulsion = (thrust * input.thrustInput) - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
        rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
    }

    public float GetSpeedPercent()
    {
        return rb.velocity.magnitude / terminalVelocity;
    }
}
