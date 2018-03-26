using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLP_HoverMotor : MonoBehaviour {
    private AudioSource source;
    bool itsPlaying;

    bool start = false;

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
    public float turnForce = 10f;

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

    //Path vars
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    private Vector3 respawnPos;
    private Quaternion respawnRotation;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        itsPlaying = false;
        //instantiate input
        input = GetComponent<PlayerInput>();
        //set rigidbody
        rb = GetComponent<Rigidbody>();
        //set drag value
        drag = thrust / terminalVelocity;

        //create layer from everything not in characters layer.
        layerMask = 1 << LayerMask.NameToLayer("Characters");
        layerMask = ~layerMask;

        //initalize path
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

    //for debuging in scene view
    private void OnDrawGizmos()
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
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal); ;

        if (grounded)
        {
            gravity = -groundNormal * hoverGrav * height;//create gravity
            hforce = groundNormal * hoverForce * hoverError;//create hover force modified by hover error.

            Gizmos.color = Color.red;
            //Gizmos.DrawLine(rb.position,rb.position * Vector3.Dot(rb.position,Vector3.up ));
            Gizmos.DrawLine(Vector3.zero,hforce);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.25f);
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
        if (start)
        {
            Speedometer.ShowSpeed(rb.velocity.magnitude, 0, 100);
            DoHover();
            Drive();
            CheckWaypointDistance();
        }
        //Speedometer.ShowSpeed(rb.velocity.magnitude, 0, 100);
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
        //if(Vector3.Distance(transform.position, nodes[currentNode].position) <= 30.0f)
        //{
        //    respawnPos = transform.position;
        //    respawnRotation = transform.rotation;
        //    print(respawnPos);
        //}

        if (Vector3.Distance(transform.position, nodes[currentNode].position) <= 35.0f)
        {
            if(currentNode == (nodes.Count - 1))
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
                respawnPos = transform.position;
                respawnRotation = transform.rotation;
                print(respawnPos);
            }
        }
        else if(currentNode != 0 && Vector3.Distance(transform.position, nodes[currentNode -1].position) > 250.0f)
        {
            //print(Vector3.Distance(transform.position, nodes[currentNode - 1].position));
            print("Out of bounds");
            transform.position = respawnPos;
            transform.rotation = respawnRotation;
        }
    }

    void Drive()
    {
        //turning
        //float rotoTorque = input.TurnInput - rb.angularVelocity.y;
        float rotoTorque = input.TurnInput;
        //rb.AddTorque(transform.up * rotoTorque * turnForce, ForceMode.Acceleration);
        //print(rb.angularVelocity.y);
        if ( Vector3.Dot(transform.up, Vector3.down) < 0.5f || Vector3.Dot(transform.up, Vector3.down) < -0.5f)
        {
            rb.AddTorque(transform.up * (rotoTorque - rb.angularVelocity.y) * turnForce, ForceMode.Acceleration);
            
        }
        else if( Vector3.Dot(transform.up, Vector3.down) >= 0.5f || Vector3.Dot(transform.up, Vector3.down) >= -0.5f)
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

        if(speed >= 1 && grounded && !itsPlaying)
        {
            
            source.Play();
            itsPlaying = true;
           
        }

        if (speed <= 1 && itsPlaying)
        {
            source.Stop();
            itsPlaying = false;
        }

 

        //forward
        float sideSpeed = Vector3.Dot(rb.velocity, transform.right);
        Vector3 sideForce = -transform.right * (sideSpeed / Time.deltaTime) / drift;
        rb.AddForce(sideForce, ForceMode.Acceleration);

        float propulsion = 0f;
        propulsion = ((input.thrustInput * thrust) - drag * Mathf.Clamp(speed, 0f, terminalVelocity));
        rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
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
        float hoverError = pid.Update((hoverHeight - height ), Time.deltaTime);//getting the error value between current height and hover height
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

}
