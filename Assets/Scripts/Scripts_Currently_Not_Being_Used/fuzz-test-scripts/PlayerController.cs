using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float horizontal, vertical, angleRotation;

    public float movementSpeed, turnSpeed, currentX, currentY;

    private Vector3 moveDirection;

    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        horizontal = vertical = currentX = currentY = angleRotation = 0.0f;
        Utilities.isStrafing = Utilities.playerRotating = false;

    }

    // FixedUpdate used for physics
    void FixedUpdate() {
        float leftTrigger = Input.GetAxis("PullUpWeapon");
        if (leftTrigger != 0) {
            ResetPlayer();
            currentX = Input.GetAxis("CameraHorizontal");
            currentY = Input.GetAxis("CameraVertical");
            transform.Rotate(Vector3.up, currentX * turnSpeed * 15.0f * Time.deltaTime);
        } else {
            Utilities.isStrafing = false;
            Utilities.playerRotating = false;
        }

        if (Utilities.isCentering) {
            horizontal = vertical = 0.0f;
        } else {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        if (!horizontal.Equals(0) || !vertical.Equals(0)) {
            AnalogMapping();
            if (!Utilities.isStrafing) {
                transform.Rotate(Vector3.up, angleRotation * Time.deltaTime * turnSpeed);
            } 
            rb.AddForce(moveDirection * movementSpeed * Time.deltaTime);
        }
    }

    // Creates directional vectors for the direction of the left analog stick, relative to the camera's position
    private void AnalogMapping() {
        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
        Vector3 cameraDirection = Utilities.FlattenCamera(Camera.main.transform.forward);

        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        moveDirection = referentialShift * stickDirection;
        angleRotation = Utilities.FindRotationAngle(moveDirection, transform.forward);
    }

    // Locks the player's forward vector to that of the camera
    private void ResetPlayer() {
        if (!Utilities.playerRotating) {
            Vector3 camDirection = Utilities.FlattenCamera(Camera.main.transform.forward);
            float angle = Utilities.FindRotationAngle(camDirection, transform.forward);
            if (angle > 1.0f || angle < -1.0f) {
                transform.Rotate(Vector3.up, angle * Time.deltaTime * turnSpeed);
            } else {
                Utilities.playerRotating = true;
            }         
        }
        else {
            Utilities.isStrafing = true;
            return;
        }
    }

}
