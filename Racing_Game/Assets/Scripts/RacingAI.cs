using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingAI : MonoBehaviour {

    bool start = false;

    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    private Vector3 respawnPos;
    private Quaternion respawnRotation;
    public float maxSteerAngle = 40f;

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
    public float thrust = 5f;//forward trust
    public float turnForce = 10f;

    //slowing factors
    public float terminalVelocity = 10f;
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
    void Start()
    {
        //instantiate input
        input = GetComponent<PlayerInput>();
        //set rigidbody
        rb = GetComponent<Rigidbody>();
        //set drag value
        drag = thrust / terminalVelocity;

        layerMask = 1 << LayerMask.NameToLayer("Characters");
        layerMask = ~layerMask;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        StartCoroutine(DoCountdown());
    }

    private void Update()
    {
        CheckWaypointDistance();
    }

    private void FixedUpdate()
    {
        speed = Vector3.Dot(rb.velocity, transform.forward);
        if (start)
        {
            DoHover();
            Drive();
            //CheckWaypointDistance();
        }
        //DoHover();
        //Drive();
        //CheckWaypointDistance();
    }

    IEnumerator DoCountdown()
    {
        yield return new WaitForSeconds(4f);
        start = true;
    }

    void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) <= 30.0f){
            respawnPos = transform.position;
            respawnRotation = transform.rotation;
            //print(respawnPos);
            if(currentNode == (nodes.Count - 1))
            {
                currentNode = 0;
            }
            else
            {
                //print("Node: "+currentNode);
                currentNode++;
                //print("Node: "+currentNode);
            }
        }
        //else if(currentNode != 0 && Vector3.Distance(transform.position, nodes[currentNode].position) > 250.0f)
        //{
        //    //print(Vector3.Distance(transform.position, nodes[currentNode - 1].position));
        //    print("Out of bounds");
        //    transform.position = respawnPos;
        //    transform.rotation = respawnRotation;
        //}
    }

    void DoHover()
    {
        Vector3 groundNormal;
        Vector3 gravity;
        Vector3 hforce;

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
            hforce = groundNormal * (hoverHeight * hoverForce) * hoverError;//create hover force modified by hover error.

            rb.AddForce(hforce, ForceMode.Acceleration);
            rb.AddForce(gravity, ForceMode.Acceleration);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));
        }
        else
        {
            gravity = -Vector3.up * fallGrav;//create gravity
            rb.AddForce(gravity, ForceMode.Acceleration);//apply gravity
            //rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));//Stabalize
        }
    }

    float CalcSteer()
    {
        Vector3 steeringVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = steeringVector.x / steeringVector.magnitude * 2f;
        //print(newSteer);
        return newSteer;
    }

    void Drive()
    {
        //turning
        //float rotoTorque = input.TurnInput - rb.angularVelocity.y;
        float rotoTorque = CalcSteer();
        //rb.AddTorque(transform.up * rotoTorque * turnForce, ForceMode.Acceleration);
        //print(rb.angularVelocity.y);
        if (Vector3.Dot(transform.up, Vector3.down) < 0.5f || Vector3.Dot(transform.up, Vector3.down) < -0.5f)
        {
            rb.AddTorque(transform.up * (rotoTorque - rb.angularVelocity.y) * turnForce, ForceMode.Acceleration);
        }
        else if (Vector3.Dot(transform.up, Vector3.down) >= 0.5f || Vector3.Dot(transform.up, Vector3.down) >= -0.5f)
        {
            rb.AddTorque(transform.up * (rotoTorque + rb.angularVelocity.y) * turnForce, ForceMode.Acceleration);
        }
        //if (input.TurnInput == 0 && Vector3.Dot(transform.up, Vector3.down) >= 0.5f || Vector3.Dot(transform.up, Vector3.down) >= -0.5f)
        //{
        //    rb.constraints = RigidbodyConstraints.FreezeRotationY;
        //}
        //else
        //{
        //    rb.constraints = RigidbodyConstraints.None;
        //}
        if (CalcSteer() == 0f)
            rb.angularVelocity *= VelocitySlowingFactor;
        if (CalcSteer() <= 0f)
            rb.velocity *= VelocitySlowingFactor;
        if (!grounded)
            return;
        //if (input.isBraking)
        //{
        //    rb.velocity *= velocityBrakingFactor;
        //}

        //forward
        float sideSpeed = Vector3.Dot(rb.velocity, transform.right);
        Vector3 sideForce = -transform.right * (sideSpeed / Time.deltaTime) / drift;
        rb.AddForce(sideForce, ForceMode.Acceleration);

        float propulsion = 0f;
        propulsion = ((1 * thrust) - drag * Mathf.Clamp(speed, 0f, terminalVelocity));
        rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
    }
}
