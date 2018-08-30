using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    private float distance = 50.0f;
    private float currentX = 0.0f;
    public Transform target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentX += Input.GetAxis("CameraHorizontal");
    }

    private void LateUpdate() {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }
}
