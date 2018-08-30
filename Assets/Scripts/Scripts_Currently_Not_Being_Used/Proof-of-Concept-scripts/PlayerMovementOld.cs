using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOld : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    Quaternion originalRotationValue;
   
    GameObject actualGolfBall;

    public float speed;
    bool goBackToRotation = false;

    // Use this for initialization
    void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        originalRotationValue = transform.rotation;

        actualGolfBall = GameObject.Find("GolfBall_Temp");
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/

        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Dead)
        {
            return;
        }

        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Vanilla)
        {
            if (goBackToRotation == true)
            {
                goBackToRotation = false;
                Quaternion newRotationValue = new Quaternion(0, 0, 0, 0);
                newRotationValue.Set(originalRotationValue.x, transform.rotation.y, originalRotationValue.x, originalRotationValue.w);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotationValue, Time.time * 1.0f);
            }

            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
            transform.Translate(0, 0, y);
            transform.Translate(x * 2.0f, 0, 0);

            float z = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
            transform.Rotate(0, z * 4.0f, 0);
            
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Weapon)
        {
            goBackToRotation = true;

            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
            transform.Translate(x, 0, y);

            //this needs to rotate through the values so that if you are facing forward it is not as senstive as turning around quickly
            float z = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
            float h = Input.GetAxis("CameraVertical") * Time.deltaTime * 35.0f;
            transform.Rotate(h , z * 7.0f, 0);
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingOutsideRing)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
            transform.Translate(0, 0, y);
            transform.Rotate(0, x * 6.0f, 0);
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingInsideRing)
        {
            //???
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
            transform.Translate(0, 0, y);
            transform.Rotate(0, x * 6.0f, 0);
            //transform.RotateAround(actualGolfBall.transform.position, Vector3.up, speed * Time.deltaTime);
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.BallFlying)
        {
            if (playerManagerScript.ShowCurrentClub() == Player_Manager.CurrentClub.Putter)
            {
                float x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
                //float y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;
                //actualGolfBall.transform.Translate(0, 0, y);
                //actualGolfBall.transform.Translate(0, 0, x * 10.0f);
                //actualGolfBall.transform.Rotate(x * 10.0f, 0, 0);
            }
            else
            {
                //REMEMBER HOW TO RESEARCH TO MAKE THE GAME FEEL LIKE IT IS IN SLOW MOTION LIKE HEAVENLY SWORD
                float x = Input.GetAxis("Horizontal") * Time.deltaTime * 35.0f;
                float y = Input.GetAxis("Vertical") * Time.deltaTime * 35.0f;
                actualGolfBall.transform.Translate(x, 0, y);
                actualGolfBall.transform.Rotate(0, x * 2.0f, 0);
            }
        }
        else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.BirdsEye)
        {
            
        }

    }
}
