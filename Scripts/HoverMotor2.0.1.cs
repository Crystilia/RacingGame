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
        crb.constraints = RigidbodyConstraints.FreezeRotationZ;
        //StartCoroutine(Stabalize(0.1f));
    }
	
	// Update is called once per frame
	void Update () {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal") * 60.0f;

        Quaternion target = Quaternion.Euler(0, turnInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2.0f);
        //Stabalize();
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (transform.rotation.x > 0)
        {
            transform.Rotate(new Vector3(0,0,45) * Time.deltaTime);
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
        crb.AddRelativeForce(0f, 0f, thrustInput * speed);
        //crb.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);
    }

    //private void Stabalize()
    //{
    //    if(transform.rotation.x > 0)
    //    { 
    //        transform.Rotate(Vector3.down * Time.deltaTime);
    //    }

    //    if(transform.rotation.x < 0)
    //    {
    //        transform.Rotate(Vector3.up * Time.deltaTime);
    //    }
    //    if(transform.rotation.z < 0)
    //    {
    //        transform.Rotate(Vector3.left * Time.deltaTime);

    //    }
    //    if(transform.rotation.z > 0)
    //    {
    //        transform.Rotate(Vector3.right * Time.deltaTime);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        //StopCoroutine(Stabalize(0.1f));
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


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Jump")
        {
            //StartCoroutine(Stabalize(1f));
            //print("constrain");
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
            //StartCoroutine(Stabalize(1f));
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.time*0.01);
            //transform.rotation = Quaternion.identity;
            crb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }
    //IEnumerator Stabalize(float delay)
    //{
    //    while (transform.rotation.x < 0)
    //    { 
    //        transform.Rotate(Vector3.down * Time.deltaTime);
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    while (transform.rotation.x > 0)
    //    {
    //        transform.Rotate(Vector3.up * Time.deltaTime);
    //        yield return new WaitForSeconds(.1f);
    //    }
    //    while (transform.rotation.z < 0)
    //    {
    //        transform.Rotate(Vector3.left * Time.deltaTime);
    //        yield return new WaitForSeconds(.1f);
    //    }

    //    while (transform.rotation.z > 0)
    //    {
    //        transform.Rotate(Vector3.right * Time.deltaTime);
    //        yield return new WaitForSeconds(.1f);
    //    }

    //    //print("Stabalizing");
    //    //if (transform.rotation.x < 0)
    //    //{
    //    //    transform.Rotate(Vector3.down*Time.deltaTime);
    //    //    //transform.Rotate(Vector3.up);
    //    //}
    //    //else if(transform.rotation.x > 0)
    //    //{
    //    //    transform.Rotate(Vector3.up * Time.deltaTime);
    //    //}
    //    //else if (transform.rotation.z < 0)
    //    //{
    //    //    transform.Rotate(Vector3.left * Time.deltaTime);
    //    //}
    //    //else if(transform.rotation.z > 0)
    //    //{
    //    //    transform.Rotate(Vector3.right * Time.deltaTime);
    //    //}
    //    //yield return new WaitForSeconds(delay);
    //    //StartCoroutine(Stabalize(.1f));
    //    // do
    //    // {
    //    //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    //    //yield return null;
    //    // } while (transform.rotation != targetRotation);
    //    //yield return new WaitForSeconds(1f);
    //    //crb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    //    //yield return null;
    //}

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
