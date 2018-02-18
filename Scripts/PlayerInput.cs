using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public string verticalAxisName = "Vertical";
    public string horizontalAxisName = "Horizontal";
    public string brakingKey = "Brake";

    [HideInInspector] public float thrustInput;
    [HideInInspector] public float TurnInput;
    [HideInInspector] public bool isBraking;
	
	// Update is called once per frame
	void Update () {
        thrustInput = Input.GetAxis(verticalAxisName);
        TurnInput = Input.GetAxis(horizontalAxisName);
        isBraking = Input.GetButton(brakingKey);
	}
}
