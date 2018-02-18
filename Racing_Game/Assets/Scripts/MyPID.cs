[System.Serializable]

public class MyPID {
    public float p = 0.2f;
    public float i = 0.05f;
    public float d = 1f;
    public float result = 0;

    private float previousError;
    private float integral;

    public float Update(float error, float dt)
    {
        float derivative = (error - previousError) / dt;
        integral += error * dt;
        previousError = error;

        result = UnityEngine.Mathf.Clamp01(p * error + i * integral + d * derivative);
        return result;

    }
}
