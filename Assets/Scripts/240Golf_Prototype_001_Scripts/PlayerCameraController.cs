using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    private Player_Input_Manager_PP_001 playerScript;
    public GameObject player;       //Public variable to store a reference to the player game object
    public Camera cam;
    public GameObject camDeathPosition;
    public GameObject camAlivePosition;
    public GameObject camGunPosition;

    private float camX;
    private float camY;

    public float normFov;
    public float aimFov;
    public float golfFov;
    public float deadFov;
    public float deathFovSpeed;
    public float deathRotateSpeed;
    public float movementSpeed;

    private bool doCamRotationOnce = true;
    private Vector3 semiTransformUp;
    private Vector3 semiTransformForward;
    private float t = 0.0f;

    private float x = 0.0f;
    private Quaternion qStart;

    public float startCamView = 30.0f;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        playerScript = player.GetComponent<Player_Input_Manager_PP_001>();
        semiTransformUp = new Vector3(0, -1.0f, 0);
        semiTransformForward = new Vector3(0, 0, 0);
        player.GetComponent<AudioSource>().Play();
        _GM.instance.TurnOffMainCamera();
        qStart = transform.rotation;
        //print(qStart);
    }

    void Update()
    {
        camX = Input.GetAxis("CameraHorizontal");
        camY = Input.GetAxis("CameraVertical");
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {}

    public void CameraStartingCamera(GameObject start, GameObject normal)
    {
        //cam.transform.position = Vector3.MoveTowards(start.transform.position, normal.transform.position, 35.5f * Time.deltaTime);
        cam.transform.position = normal.transform.position;   
    }

    public void CameraStartView(float movementSpeed)
    {
        if (cam.fieldOfView != startCamView)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, startCamView, movementSpeed * Time.deltaTime);
        }
    }

    public void CameraVanilla()
    {
        if (cam.fieldOfView != normFov)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normFov, movementSpeed * Time.deltaTime);//
        }

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camAlivePosition.transform.position, 5.0f * Time.deltaTime);
    }

    public void CameraGun()
    {
        int bullets = transform.parent.GetComponent<Player_Manager_PP_001>().GetSinglesBullets() + 1;

        if (bullets <= 0)
        {
            aimFov = 30.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            return;
        }

        //cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);

        if (bullets == 10)
        {
            aimFov = 25.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);
            return;
        }

        if (bullets == 9)
        {
            aimFov = 26.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);
            return;
        }

        if (bullets == 8)
        {
            aimFov = 27.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);
            return;
        }

        if (bullets == 7)
        {
            aimFov = 28.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);
            return;
        }

        if (bullets == 6)
        {
            aimFov = 29.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camGunPosition.transform.position, 5.0f * Time.deltaTime);
            return;
        }

        if (bullets <= 5)
        {
            //The directional vector can be determined by subtracting the start from the terminal point.
            aimFov = 30.0f;
            if (cam.fieldOfView != aimFov) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, movementSpeed * Time.deltaTime);
            Vector3 u = camAlivePosition.transform.position - camGunPosition.transform.position;
            u.Normalize();
            float dist = Vector3.Distance(camGunPosition.transform.position, camAlivePosition.transform.position);
            dist = dist / bullets;
            u = u * dist;
            u = u + camGunPosition.transform.position;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, u, 5.0f * Time.deltaTime);
        }
    }

    public void CameraGolfFree()
    {
        if (cam.fieldOfView != golfFov)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, golfFov, movementSpeed * Time.deltaTime);
        }
    }

    public void CameraGolfFree_Start()
    {
        //i like this code so please don't erase just yet.  
        /*print("ARe we here 1");
        x = x - 1 * Time.deltaTime;
        Vector3 rotation = new Vector3(cam.transform.eulerAngles.x - x * Time.deltaTime, 0, 0);
        if (x > -1.0f)
        {
            transform.Rotate(x, 0, 0);
        }*/
        

        //Quaternion centeredRotation = Quaternion.Euler(75.0f, transform.eulerAngles.y, transform.eulerAngles.z);

        //smoothly rotate the camera down towards the centered angle which means user wants to look at ground via c hotkey
        //transform.rotation = Quaternion.Slerp(transform.rotation, centeredRotation, Time.deltaTime * 2.0f);


        if (cam.fieldOfView != golfFov)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, golfFov, movementSpeed * Time.deltaTime);
        }

        
        //transform.Rotate(-x, 0, 0);
    }

    public void CameraFastTravelMap()
    {
        _GM.instance.TurnOnFastTravelCamera();
        cam.enabled = false;
    }

    public void CameraPlayerDeadRise()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, deadFov, deathFovSpeed * Time.deltaTime);

        //transform.RotateAround(player.transform.position, Vector3.up, deathRotateSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, camDeathPosition.transform.position, 0.5f * Time.deltaTime);
        Vector3 topDownVec = Vector3.RotateTowards(transform.forward, semiTransformUp, 0.2f * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(topDownVec);
    }

    public void CameraPlayerDeadFall()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, deadFov - 40.0f, deathFovSpeed / 2.0f * Time.deltaTime);
    }

    public void CameraPlayerDeadReset()
    {
        transform.position = Vector3.MoveTowards(transform.position, camAlivePosition.transform.position, 2.0f * Time.deltaTime);
        Vector3 topDownVec = Vector3.RotateTowards(transform.forward, player.transform.forward, .5f * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(topDownVec);
    }

    public void TurnOffFastTravelCamera()
    {
        cam.enabled = true;
        _GM.instance.TurnOffFastTravelCamera();
    }
}
