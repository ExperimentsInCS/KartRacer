using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour {
    [SerializeField] private WheelCollider[] powerColliders;
    [SerializeField] private WheelCollider[] steerColliders;
    [SerializeField] private WheelCollider[] brakeColliders;

    [SerializeField] private Transform[] steerTransforms;
    [SerializeField] private Transform[] nonSteerTransforms;
    [SerializeField] private Transform steeringWheelTransform;

    [SerializeField] private float wheelRotationFactor = 0.0f;

    private float powerTorque = 0.0f;
    private float steerAngle = 0.0f;
    private float brakeTorque = 0.0f;
    private float rotationAngle = 0.0f;

    [SerializeField] float currentSpeed = 0.0f;

    [SerializeField] float coastTorque = 50000f;

    private Rigidbody rb = null;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Steer(float angle) {
        steerAngle = angle;
    }

    public void Power(float powerAmount) {
        powerTorque = powerAmount;
        brakeTorque = 0.0f;
    }

    public void Brake(float brakeAmount) {
        Debug.Log("brakeAmount (braking): " + brakeAmount);
        brakeTorque = brakeAmount;
        powerTorque = 0.0f;
    }

    public void Coast() {
        Debug.Log("brakeAmount (coasting): " + coastTorque);
        brakeTorque = coastTorque;
        powerTorque = 0.0f;
    }

    private void Update() {
        currentSpeed = rb.velocity.magnitude;

        float rotation = currentSpeed * Time.deltaTime * wheelRotationFactor;
        rotationAngle += rotation;

        UpdateSteeringWheel();

        UpdateControlWheels();
        UpdateNonControlWheels();

    }

    private void UpdateSteeringWheel() {
        float xRot = steeringWheelTransform.localRotation.eulerAngles.x;
        float yRot = steeringWheelTransform.localRotation.eulerAngles.y;
        steeringWheelTransform.localRotation = Quaternion.Euler(xRot, yRot, steerAngle * 3.0f);

    }

    private void UpdateControlWheels() {
        foreach (Transform wheel in steerTransforms) {
            float xRot = wheel.localRotation.eulerAngles.x;
            wheel.localRotation = Quaternion.Euler(xRot, steerAngle, rotationAngle);
        }
    }

    private void UpdateNonControlWheels() {
        foreach (Transform wheel in nonSteerTransforms) {
            float xRot = wheel.localRotation.eulerAngles.x;
            float yRot = wheel.localRotation.eulerAngles.y;
            wheel.localRotation = Quaternion.Euler(xRot, yRot, rotationAngle);
        }
    }

    private void FixedUpdate() {
        // add acceleration torque
        foreach (WheelCollider wheel in powerColliders) {
            wheel.motorTorque = -powerTorque;
        }

        // add reverse torque caused by the brakes
        foreach (WheelCollider wheel in brakeColliders) {
            wheel.brakeTorque = brakeTorque;
        }

        // add steering
        foreach (WheelCollider wheel in steerColliders) {
            wheel.steerAngle = steerAngle;
        }
    }
}
