using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha_HoverMotor : MonoBehaviour {
    //static input class attached to game object.
    PlayerInput input;

    //my PID: for stabalizing the hover.
    public MyPID pid;

    //rigidbody. 
    Rigidbody rb;

    //bool for testing if we are grounded
    bool grounded;

    //floats for setting max hover height that we stay grounded at
    //and the height we want to activly hover at
    public float hoverHeight;
    public float hoverHeightMax;

    public float hoverForce;

    //container for current speed; seperate from thrust
    //used to hold active speed value and be modified by drag and slow.
    public float speed;

    //driving vars
    public float thrust = 75f;//forward trust

    //slowing factors
    public float terminalVelocity = 60;
    public float VelocitySlowingFactor = 0.99f;
    public float velocityBrakingFactor = 0.95f;
    public float drift = 4;
    float drag;
    

    //gravity values
    public float hoverGrav = 1f;
    public float fallGrav = 30f;

    //set not ~Characters a "Characters" layer must exist. 
    int layerMask;

    // Use this for initialization
    void Start () {
        //instantiate input
        input = GetComponent<PlayerInput>();
        //set rigidbody
        rb = GetComponent<Rigidbody>();
        //set drag value
        drag = thrust / terminalVelocity;

        layerMask = 1 << LayerMask.NameToLayer("Characters");
        layerMask = ~layerMask;
    }

    //for debuging in scene view
    private void OnDrawGizmos()
    {
        Vector3 groundNormal;
        Vector3 gravity;
        Vector3 force;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        grounded = Physics.Raycast(ray, out hit, hoverHeightMax, layerMask);

        float height = hit.distance;
        groundNormal = hit.normal.normalized;

        if (grounded)
        {
            gravity = -Vector3.up;
            force = Vector3.up;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.5f);
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update () {
        
	}

    private void FixedUpdate()
    {
        speed = Vector3.Dot(rb.velocity, transform.forward);
        DoHover();
        Drive();
    }

    void DoHover()
    {
        Vector3 groundNormal;
        Vector3 gravity;
        Vector3 force;

        //raycast vars
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        
        //raycast to grounded
        grounded = Physics.Raycast(ray, out hit, hoverHeightMax, layerMask);
        
        float height = hit.distance;//distance from ground
        float hoverError = pid.Update((hoverHeight - height), Time.deltaTime);//getting the error value between current height and hover height
        groundNormal = hit.normal.normalized;//get the normal of the ground

        //create projection of ground normal for rotating the vehicle
        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

        //hover
        if (grounded)
        {
            gravity = -groundNormal * hoverGrav * height;//create gravity
            force = groundNormal * hoverForce * hoverError;//create hover force modified by hover error.

            rb.AddForce(force, ForceMode.Acceleration);
            rb.AddForce(gravity, ForceMode.Acceleration);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 5f));
        }
        else
        {
            gravity = -Vector3.up * fallGrav;//create gravity
            rb.AddForce(gravity, ForceMode.Acceleration);//apply gravity
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 5f));//Stabalize
        }
    }

    void Drive()
    {
        //turning
        float rotoTorque = input.TurnInput - rb.angularVelocity.y;
        print(input.TurnInput);
        rb.AddTorque(transform.up * rotoTorque * 10f, ForceMode.Acceleration);

        if(input.TurnInput == 0 && rb.rotation.eulerAngles.x > 90|| rb.rotation.eulerAngles.z > 90)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        if (input.TurnInput == 0f)
            rb.angularVelocity *= VelocitySlowingFactor;
        if (input.thrustInput <= 0f)
            rb.velocity *= VelocitySlowingFactor;
        if (!grounded)
            return;
        if (input.isBraking)
        {
            rb.velocity *= velocityBrakingFactor;
        }

        //forward
        float sideSpeed = Vector3.Dot(rb.velocity, transform.right);
        Vector3 sideForce = -transform.right * (sideSpeed / Time.deltaTime) / drift;
        rb.AddForce(sideForce, ForceMode.Acceleration);

        float propulsion = 0f;
        propulsion = ((input.thrustInput * thrust) - drag * Mathf.Clamp(speed, 0f, terminalVelocity));
        rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
    }

    public float GetSpeedPercent()
    {
        return rb.velocity.magnitude / terminalVelocity;
    }
}
