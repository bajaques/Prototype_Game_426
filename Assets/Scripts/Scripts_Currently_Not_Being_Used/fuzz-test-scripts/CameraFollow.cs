using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    Camera mainCam;

    public Transform parent, camTransform, target, aimTarget;

    public float camDistance, revolveSpeed;
    private float currentX, currentY, aimX, aimY, centerCamAngle;

    private bool firstAngleFound;

    [SerializeField] private float cameraSpeed;

    // Initialize fields
    private void Start() {
        mainCam = Camera.main;
        camTransform = mainCam.transform;
        parent = camTransform.parent;
        aimTarget = parent.Find("AimTarget");
        target = parent;
        cameraSpeed = 100.0f;


        Utilities.isCentering = Utilities.isZooming = firstAngleFound = false;

        currentX = currentY = centerCamAngle = 0.0f;
    }

    // Get input from controller
    private void Update() {
        if (Input.GetAxis("PullUpWeapon") != 0) {
            Utilities.isZooming = true;
        } else {
            target = parent;
            Camera.main.fieldOfView = 60.0f;
            Utilities.isZooming = false;
            if (Input.GetButtonDown("CenterCamera")) {
                Utilities.isCentering = true;
            } else {
                currentX += Input.GetAxis("CameraHorizontal");
                currentX = currentX % 360.0f;
                currentY += Input.GetAxis("CameraVertical");
                currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
        }
    }

    // Late update used to follow after player's update method
    private void LateUpdate() {
        if (Utilities.isZooming && Utilities.playerRotating) {
            target = aimTarget;
            //Vector3 behindTarget = target.position - target.forward * camDistance;
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 45.0f, 40 * Time.deltaTime);
            camTransform.position = target.position - (target.forward * camDistance);
            camTransform.LookAt(target.position);
        }
        else if (Utilities.isCentering) {
            ResetCamera();
            if (Vector3.Angle(Utilities.FlattenCamera(camTransform.forward), target.forward) < 1.0f) {
                Utilities.isCentering = firstAngleFound = false;
                currentX += centerCamAngle / 2;
            }
        } else {
            Vector3 direction = new Vector3(0, 0, -camDistance);
            Quaternion rotation = Quaternion.Euler(currentY * cameraSpeed * Time.fixedDeltaTime, currentX * cameraSpeed * Time.fixedDeltaTime, 0);
            camTransform.position = target.position + rotation * direction;
            camTransform.LookAt(target.position);
        }
    }

    // Moves camera behind player
    private void ResetCamera() {
        Vector3 camForward = Utilities.FlattenCamera(camTransform.forward);
        float angle = Utilities.FindRotationAngle(target.forward, camForward);
        camTransform.RotateAround(target.position, Vector3.up, angle * revolveSpeed * Time.deltaTime);

        if (!firstAngleFound) {
            centerCamAngle = angle;
            firstAngleFound = true;
        }
    }
}
