using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

	public Transform target;
	public bool autoTargetPlayer;
	public LayerMask wallLayer;

	public enum Shoulder {
		Right, Left
	}
	public Shoulder shoulder;

	[System.Serializable]
	public class CameraSettings {
		[Header("-Positioning-")]
		public Vector3 camPositionOffsetLeft;
		public Vector3 camPositionOffsetRight;

		[Header("-Camera Options-")]
		public float mouseXSensitivity = 5.0f;
		public float mouseYSensitivity = 5.0f;
		public float minAngle = -60.0f;
		public float maxAngle = 70.0f;
		public float rotationSpeed = 5.0f;

		[Header("-Zoom-")]
		public float fieldOfView = 70.0f;
		public float zoomFieldOfView = 30.0f;
		public float zoomSpeed = 3.0f;

		[Header("-Visual Options-")]
		public float hideMeshWhenDistance = 0.5f;
	}
	[SerializeField]
	CameraSettings cameraSettings;

	[System.Serializable]
	public class InputSettings {
		public string verticalAxis = "CameraVertical";
		public string horizontalAxis = "CameraHorizontal";
		public string aimButton = "Aiming";
		//public string switchShoulderButton = "";	
	}
	public InputSettings input;

	[System.Serializable]
	public class MovementSettings {
		public float movementLerpSpeed = 5.0f;
	}
	[SerializeField]
	public MovementSettings movement;

	Transform pivot;
	Camera cam;
	float newX = 0.0f;
	float newY = 0.0f;


	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("PlayerCamera").GetComponent<Camera>();
		pivot = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			RotateCamera();
			// Check collisions
			Zoom(Input.GetAxis(input.aimButton));

			// Switch shoulders ?
		}
	}

	void LateUpdate() {
		if (!target) {
			TargetPlayer();
		} else {
			Vector3 targetPosition = target.position;
			Quaternion targetRotation = target.rotation;
			FollowTarget(targetPosition, targetRotation);
		}

	}

	// Finds the player gameObject and sets as target
	void TargetPlayer() {
		if (autoTargetPlayer) {
			GameObject player = GameObject.FindGameObjectWithTag("PlayerCharacter");

			if (player) {
				Transform playerT = player.transform;
				target = playerT;
			}
		}
	}

	// Following the Target
	void FollowTarget(Vector3 targetPosition, Quaternion targetRotation) {
		Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movement.movementLerpSpeed);
		transform.position = newPos;
	}

	// Rotates with input
	void RotateCamera() {
		if (!pivot) {
			return;
		}

		newX += cameraSettings.mouseXSensitivity * Input.GetAxis(input.verticalAxis);
		newY += cameraSettings.mouseYSensitivity * Input.GetAxis(input.horizontalAxis);

		Vector3 eulerAngleAxis = new Vector3();
		eulerAngleAxis.x = newX;
		eulerAngleAxis.y = newY;

		newY = Mathf.Repeat(newY, 360);
		newX = Mathf.Clamp(newX, cameraSettings.minAngle, cameraSettings.maxAngle);

		Quaternion newRotation = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);

		pivot.localRotation = newRotation;
	}

	// Zooms the camera in
	void Zoom(float isZooming) {
		if (!cam) {
			return;
		}

		if (isZooming != 0) {
			float newFieldOfView = Mathf.Lerp(cam.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
			cam.fieldOfView = newFieldOfView;
		} else {
			float originalFieldOfView = Mathf.Lerp(cam.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
			cam.fieldOfView = originalFieldOfView;
		}
	}
}
