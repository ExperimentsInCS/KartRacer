using UnityEngine;

[RequireComponent(typeof(KartController))]
public class PlayerInput : MonoBehaviour {
    [SerializeField] private float acceleration = 20.0f;
    [SerializeField] private float steeringAngle = 20.0f;
    [SerializeField] private float brakeFactor = -2500.0f;

    private KartController controller = null;

    private void Awake() {
        controller = GetComponent<KartController>();
    }

    private void Update() {
        float steerAmount = Input.GetAxis("Horizontal") * steeringAngle;
        float powerAmount = Input.GetAxis("Vertical") * acceleration;

        if (powerAmount < 0.0f) {
            // brake
            controller.Brake(powerAmount * brakeFactor);
        } else if (powerAmount > 0.0f) {
            // accelerate
            controller.Power(powerAmount);
        } else {
            controller.Coast();
        }

        controller.Steer(steerAmount);
    }
}
