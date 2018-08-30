using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    GameObject GolfBallCamera;
    GameObject OverWorldCamera;
    GameObject GolfBallCamera2;

    GameObject actualPlayer;
    GameObject actualGolfBall;

    private float GolfClubFOV; // field of view for golf clubs;
    private float WeaponFOV; //field of view for guns
    private float VanillaFOV;
    private float currentFOV;

    GameObject VanillaCameraFollow;
    GameObject GolfingCameraFollow;
    GameObject WeaponCameraFollow;

    bool horAndVertCameraMoving = false;
    bool horAndVertCameraMoved = false;
    float previousX = 0.0f;
    float preciousY = 0.0f;

    // Use this for initialization
    void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        GolfBallCamera = GameObject.Find("GolfBallTempCamera");
        OverWorldCamera = GameObject.Find("OverWorldCamera");
        GolfBallCamera2 = GameObject.Find("GolfBallLanding_Camera");

        GolfBallCamera.SetActive(false);
        OverWorldCamera.SetActive(false);
        GolfBallCamera2.SetActive(false);

        actualPlayer = GameObject.Find("SmallHuman_001");
        actualGolfBall = GameObject.Find("GolfBall_Temp");

        GolfClubFOV = 120.0f;
        WeaponFOV = 100.0f;
        VanillaFOV = 60.0f;
        currentFOV = GetComponent<Camera>().fieldOfView;

        VanillaCameraFollow = GameObject.Find("VanillaCameraFollow");
        GolfingCameraFollow = GameObject.Find("GolfingCameraFollow");
        WeaponCameraFollow = GameObject.Find("WeaponCameraFollow");

        horAndVertCameraMoving = false;
        horAndVertCameraMoved = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Dead)
        {
            return;
        }

        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Vanilla)
        {
            GolfBallCamera.SetActive(false);
            OverWorldCamera.SetActive(false);
            GolfBallCamera2.SetActive(false);

            Vector3 actualPlayerPosition = actualPlayer.transform.position;
            Vector3 eye = actualPlayer.transform.position - actualPlayer.transform.forward * -100.0f + actualPlayer.transform.up * + 25.0f;
            Vector3 cameraForward = actualPlayer.transform.position - eye;
            cameraForward.Normalize();
            Vector3 cameraLeft = Vector3.Cross(actualPlayer.transform.up, cameraForward).normalized;
            Vector3 cameraUp = Vector3.Cross(cameraForward, cameraLeft).normalized;
            transform.position = Vector3.MoveTowards(transform.position, VanillaCameraFollow.transform.position, 50.0f * Time.deltaTime);
            transform.LookAt(eye, cameraUp);

            //Camera FOV
            if (currentFOV != VanillaFOV)
            {
                if (currentFOV < VanillaFOV)
                {
                    GetComponent<Camera>().fieldOfView = currentFOV + 1.0f;
                    currentFOV = GetComponent<Camera>().fieldOfView;
                }
                else
                {
                    GetComponent<Camera>().fieldOfView = currentFOV - 1.0f;
                    currentFOV = GetComponent<Camera>().fieldOfView;
                }
            }

        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Weapon)
        {
            Vector3 actualPlayerPosition = actualPlayer.transform.position;
            Vector3 eye = actualPlayer.transform.position - actualPlayer.transform.forward * -100.0f + actualPlayer.transform.up * +25.0f;
            Vector3 cameraForward = actualPlayer.transform.position - eye;
            cameraForward.Normalize();
            Vector3 cameraLeft = Vector3.Cross(actualPlayer.transform.up, cameraForward).normalized;
            Vector3 cameraUp = Vector3.Cross(cameraForward, cameraLeft).normalized;
            transform.position = Vector3.MoveTowards(transform.position, WeaponCameraFollow.transform.position, 50.0f * Time.deltaTime);
            transform.LookAt(eye, cameraUp);

            //Camera FOV
            if (currentFOV < WeaponFOV)
            {
                //currentFOV = currentFOV + 1.0f;
                GetComponent<Camera>().fieldOfView = currentFOV + 1.0f;
                currentFOV = GetComponent<Camera>().fieldOfView;
            }

            /*Vector3 actualPlayerPosition = actualPlayer.transform.position;
            actualPlayerPosition.x = actualPlayerPosition.x + 0.5f;
            actualPlayerPosition.y = actualPlayerPosition.y + 3.0f;
            actualPlayerPosition.z = actualPlayerPosition.z + 4.0f;
            transform.position = Vector3.MoveTowards(transform.position, actualPlayerPosition, 75.0f * Time.deltaTime);

            //Camera Movement
            float x = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
            float y = Input.GetAxis("CameraVertical") * Time.deltaTime * 35.0f;
            transform.Rotate(y * 2.0f, x * 2.0f, 0);*/

        }
        else if ((playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingOutsideRing) ||
                (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingInsideRing))
        {
            if (currentFOV > GolfClubFOV)
            {
                GetComponent<Camera>().fieldOfView = currentFOV - 1.0f;
                currentFOV = GetComponent<Camera>().fieldOfView;
            }

            Vector3 actualPlayerPosition = actualPlayer.transform.position;
            Vector3 eye = actualPlayer.transform.position - actualPlayer.transform.forward * -100.0f + actualPlayer.transform.up * -10.0f;
            Vector3 cameraForward = actualPlayer.transform.position - eye;
            cameraForward.Normalize();
            Vector3 cameraLeft = Vector3.Cross(actualPlayer.transform.up, cameraForward).normalized;
            Vector3 cameraUp = Vector3.Cross(cameraForward, cameraLeft).normalized;
            transform.position = Vector3.MoveTowards(transform.position, GolfingCameraFollow.transform.position, 50.0f * Time.deltaTime);

            if ((horAndVertCameraMoving == false) && (horAndVertCameraMoved == false))
            {
                transform.LookAt(eye, cameraUp);
            }
            if ((horAndVertCameraMoving == false) && (horAndVertCameraMoved == true))
            {
                //Quaternion rotation = Quaternion.LookRotation(actualPlayer.transform.position - transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                //transform.rotation = Quaternion.Slerp
            }


            float x = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
            if (x != 0.0f)
            {
                previousX = x;
                horAndVertCameraMoving = true;
                transform.Rotate(0, x * 1.0f, 0);
            }

            if ((x == 0.0f) && (previousX != 0.0f))
            {
                horAndVertCameraMoving = false;
                horAndVertCameraMoving = true;

            }

            if ((x == 0.0f) && previousX == 0.0f)
            {
                horAndVertCameraMoving = false;
                horAndVertCameraMoved = false;
            }

            //print(x);
            //float y = Input.GetAxis("CameraVertical") * Time.deltaTime * 35.0f;
            //transform.Rotate(y * 0.5f, x * 0.5f, 0);
        }

        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.BallFlying)
        {
            if (playerManagerScript.ReturnGolfBallHasStruckTerrain() == true)
            {
                GolfBallCamera2.SetActive(true);
                Vector3 pos = actualGolfBall.transform.position;
                pos.z -= 5.0f;
                pos.y += 50.0f;
                GolfBallCamera2.transform.position = pos;
                GolfBallCamera2.transform.RotateAround(actualGolfBall.transform.position, -Vector3.up, 32.0f * Time.deltaTime);
                GolfBallCamera.SetActive(false);
            }
            else
            {
                if(playerManagerScript.ShowCurrentClub() != Player_Manager.CurrentClub.Putter)
                {
                    GolfBallCamera2.SetActive(false);
                    GolfBallCamera.SetActive(true);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, actualGolfBall.transform.position, 100.0f * Time.deltaTime);
                }
            }
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.BirdsEye)
        {
            OverWorldCamera.SetActive(true);
        }
    }
}





//this was previous golfball flying algoirthm, might need again - 20180106 - Brian
// Vector3 pos = actualPlayer.transform.position;
//Vector3 pos = actualGolfBall.transform.position;
//pos.z += 20.0f;
//transform.position = pos;
//GolfBallCamera.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
/*Vector3 GolfBallCameraPosition = actualGolfBall.transform.position;
Vector3 eye = actualGolfBall.transform.position - actualGolfBall.transform.forward * -30.0f + actualGolfBall.transform.up * -10.0f;
Vector3 cameraForward = actualGolfBall.transform.position - eye;
cameraForward.Normalize();
Vector3 cameraLeft = Vector3.Cross(actualGolfBall.transform.up, cameraForward).normalized;
Vector3 cameraUp = Vector3.Cross(cameraForward, cameraLeft).normalized;
GolfBallCamera.transform.position = Vector3.MoveTowards(actualGolfBall.transform.position, GolfingCameraFollow.transform.position, 50.0f * Time.deltaTime);
GolfBallCamera.transform.LookAt(eye, cameraUp);*/

//pos.z += 20.0f; first person mode for the ball flying, kinda cool and something to think about!!!
//pos.z -= 20.0f; wider view of flying behind ball.  I kinda like this too!!!
// pos.z -= 10.0f; get the opinion from the fellas on these different views
//pos.z -= 5.0f;
