using UnityEngine;
using UnityEngine.UI; // Include this for UI components

public class wheelsController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider rearRight;
    [SerializeField] WheelCollider rearLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform rearRightTransform;
    [SerializeField] Transform rearLeftTransform;

    [SerializeField] Text speedometerText; // Reference to the UI Text element

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;
    public float maxSpeed = 50f;

    private float accelerationDecreaseThreshold = 0.8f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;
    private Rigidbody rb;

    //brake lights
    public Material brakeMaterial;
    public Color brakingColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float inputAcceleration = Input.GetAxis("Vertical");

        float currentSpeed = rb.velocity.magnitude;

        if (currentSpeed >= maxSpeed * accelerationDecreaseThreshold)
        {
            // Scaling acceleration based on current speed
            float speedFactor = (currentSpeed - maxSpeed * accelerationDecreaseThreshold) / (maxSpeed - maxSpeed * accelerationDecreaseThreshold);
            currentAcceleration = Mathf.Lerp(acceleration * inputAcceleration, acceleration * inputAcceleration * (1 - speedFactor), speedFactor);
        }
        else
        {
            currentAcceleration = acceleration * inputAcceleration;
        }

        // Speed limit
        if (currentSpeed > maxSpeed)
        {
            currentAcceleration = 0f; // Stops accelerating if at or above max speed
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakingForce;
        }
        else
        {
            currentBreakForce = 0f;
        }

        //torque and brake force
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        rearRight.brakeTorque = currentBreakForce;
        rearLeft.brakeTorque = currentBreakForce;

        // Steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        // wheel animation
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(rearLeft, rearLeftTransform);
        UpdateWheel(rearRight, rearRightTransform);

        //speedometer
        if (speedometerText != null)
        {
            speedometerText.text = $"Speed: {Mathf.RoundToInt(currentSpeed * 3.6f)} km/h";
        }
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
}
