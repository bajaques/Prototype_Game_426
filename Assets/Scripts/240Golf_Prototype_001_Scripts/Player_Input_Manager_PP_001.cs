using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Player_Input_Manager_PP_001 : NetworkBehaviour
{
    //Recieves input from controller and decides player state accordingly, then informs every need to know script.

    public enum PlayerStatus { Vanilla, Gun, GolfFree, GolfCircle, Map, MapFastTravel, DeadRise, DeadFall, DeadReset };
    public PlayerStatus playerCurrentState;

    [SerializeField] private Player_Controller_Manager_PP_001 playerControll_Script;
    [SerializeField] private ClubWeaponManager_PP_001 clubWeapon_Script;
    [SerializeField] private PlayerCameraController playerCameraControll_Script;

    private float previousTriggerValue = 0.0f;

    public Camera myCamera;
    
    public bool alive = false;
    public float deathCountDown = 20.0f;
    public float deathFallCountDown = 5.0f;
    private bool fall = false;
    private bool toggleMusic = false;
    public bool overheadMap = false;
    private bool returnToVanilla = false;

    private bool freeGolfMode = true;
    public bool circleGolfModeBallInFlight = true;

    void Update ()
    {
        if (!isLocalPlayer)
        {
            myCamera.enabled = false;
            return;
        }

        if (alive) AliveState(); 
        else DeadState();
    }
     
    private void AliveState()
    {

        if (overheadMap)
        {
            //playerCurrentState = PlayerStatus.MapFastTravel;
            playerCameraControll_Script.CameraFastTravelMap();
            if (Input.GetKeyDown("joystick button 1"))
            {
                overheadMap = false;
                myCamera.GetComponent<PlayerCameraController>().TurnOffFastTravelCamera();
                //playerCurrentState = PlayerStatus.Vanilla;
                playerCameraControll_Script.CameraVanilla();
                playerControll_Script.resetCharacterToVanilla();
            }
            return;
        }

        if (Input.GetKeyDown("joystick button 6"))
        {
            if (toggleMusic == false)
            {
                GetComponent<AudioSource>().Pause();
                toggleMusic = true;
            }
            else
            {
                GetComponent<AudioSource>().UnPause();
                toggleMusic = false;
            }
        }

        //Driver (GolfClub and Weapon)
        if (Input.GetKeyDown("joystick button 2"))
        {
            clubWeapon_Script.SwitchToDriver();
        }

        //Iron (GolfClub and Weapon)
        if (Input.GetKeyDown("joystick button 3"))
        {
            clubWeapon_Script.SwitchToIron();
        }

        //Putter (GolfClub and Weapon)
        if (Input.GetKeyDown("joystick button 1"))
        {
            clubWeapon_Script.SwitchToPutter();
        }

        float ControllerTrigger = Input.GetAxis("PullUpWeapon");//fix this terminology

        //if (ControllerTrigger != previousTriggerValue)
        //{
        if (ControllerTrigger == 0.0f)
        {
            playerCurrentState = PlayerStatus.Vanilla;
            playerCameraControll_Script.CameraVanilla();

            if (returnToVanilla)
            {
                playerControll_Script.resetCharacterToVanilla();//look into this //
                returnToVanilla = false;
            }

            if (Input.GetKey("joystick button 8"))
            {
                playerControll_Script.RunWalkRun(true);
                playerControll_Script.RunTranslate();
                playerControll_Script.RunVanillaState(true);
            }
            else
            {
                playerControll_Script.RunWalkRun(false);
                playerControll_Script.RunTranslate();
                playerControll_Script.RunVanillaState(false);
            }
            
            clubWeapon_Script.VanillaMode();//
        }
        else if (ControllerTrigger < 0.0f)
        {
            returnToVanilla = true;
            playerControll_Script.RunWalkRun(false);
            playerControll_Script.RunTranslate();
            //playerCurrentState = PlayerStatus.Gun;
            playerCameraControll_Script.CameraGun();
            playerControll_Script.RunGunState();
            clubWeapon_Script.WeaponMode();//
        }
        else if (ControllerTrigger > 0.0f)
        {
            if (freeGolfMode == true)
            {
                //Time.timeScale = 0.5f;
                returnToVanilla = true;
                playerControll_Script.RunWalkRun(false);
                playerControll_Script.RunTranslate();
                playerCurrentState = PlayerStatus.GolfFree;
                playerCameraControll_Script.CameraGolfFree();
                playerControll_Script.RunGolfFreeState();
                clubWeapon_Script.GolfMode();
            }
            else
            {
                returnToVanilla = true;
                playerCurrentState = PlayerStatus.GolfCircle;
                //playerCameraControll_Script.CameraGolfFree();//
                playerControll_Script.RunGolfCircleState(circleGolfModeBallInFlight);
                clubWeapon_Script.GolfMode();
            }  
        }

        if (Input.GetKeyDown("joystick button 6"))
        {
            //playerCurrentState = PlayerStatus.Map;
        }

        previousTriggerValue = ControllerTrigger;
    }

    private void DeadState()
    {
        if (playerControll_Script.die == true)
        {
            //playerCurrentState = PlayerStatus.DeadRise;
            playerCameraControll_Script.CameraPlayerDeadRise();
            playerControll_Script.RunJustDiedState();
        }
        else if (playerControll_Script.dieRise == true)
        {
            //playerCurrentState = PlayerStatus.DeadRise;
            playerCameraControll_Script.CameraPlayerDeadRise();
            playerControll_Script.RunDeadRiseState();
            deathCountDown -= Time.deltaTime;
            if (deathCountDown < 0)
            {
                playerControll_Script.dieRise = false;
                deathCountDown = 20.0f;
                fall = true;
            }
        }
        else if (fall == true)
        {
            //playerCurrentState = PlayerStatus.DeadFall;
            playerCameraControll_Script.CameraPlayerDeadFall();
            playerControll_Script.RunDeadFallState();
            deathFallCountDown -= Time.deltaTime;
            if (deathFallCountDown < 0)
            {
                fall = false;
                playerControll_Script.RunComeBackAliveState();
                //playerCurrentState = PlayerStatus.DeadReset;
                playerCameraControll_Script.CameraPlayerDeadReset();
            }
        }
        /*else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.up), out hit, Mathf.Infinity))
            {
                float dist = Vector3.Distance(hit.transform.position, transform.position);
                print(dist);
                Debug.Log(hit.transform.name);
                if (dist < 400.0f)
                {
                    alive = true;
                    playerControll_Script.resetCharacterToVanilla();
                }

            }
        }*/
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            if (alive == false && deathFallCountDown < 0)
            {
                alive = true;
                
                deathFallCountDown = 20.0f;
                deathFallCountDown = 4.5f;
                playerControll_Script.resetCharacterToVanilla();
                playerControll_Script.die = true;
            }
        }

    }

   /* public void UpdateState_GolfCircle()//to be determined what we do with this
    {
        playerCurrentState = PlayerStatus.GolfFree;
    }*/

    public bool GetAliveStatus()
    {
        return alive;
    }

    public void SetAliveStatus(bool b)
    {
        alive = b;
    }

    public void ChangeFromFreeGolfToCircle(bool b)
    {
        freeGolfMode = b;
    }
}
