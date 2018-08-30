using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barleyPlayerController : MonoBehaviour {

    GameObject cameraPlayer;

	
	void Start ()
    {
        GetComponent<Animator>().enabled = false;
        //cameraPlayer = transform.Find("barleyTempCamera").gameObject;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
        transform.Translate(0, 0, y);
        transform.Translate(x * 2.0f, 0, 0);

        float z = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
        transform.Rotate(0, z * 4.0f, 0);


        if ((x != 0.0f || y != 0.0f))
        {
            GetComponent<Animator>().enabled = true;

            //cameraPlayer.transform.parent = null;
/*
            //TRIAL FUZZ CODE TO GET PLAYER TO ROTATE - WILL NEED TO MOVE CAMERA SEPERATLEY DURING THESE INSTANCES IF I WANT TO KEEP IT THIS WAY
            Vector3 stickDirection = new Vector3(x, 0, y);
            Vector3 playerDirection = transform.forward;
            Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, playerDirection);
            Vector3 moveDirection = referentialShift * stickDirection;
            Vector3 rotationAxis = Vector3.Cross(moveDirection, playerDirection);
            float angleRotation = Vector3.Angle(playerDirection, moveDirection) * (rotationAxis.y >= 0 ? -1f : 1f);
            transform.Rotate(Vector3.up, angleRotation * Time.deltaTime * 2.5f);

            //cameraPlayer.transform.parent = null;
            //cameraPlayer.transform.parent = this.transform.parent;
            */
        }
        else
        {
            GetComponent<Animator>().enabled = false;
            //cameraPlayer.transform.parent = transform;
            
        }
    }
}
