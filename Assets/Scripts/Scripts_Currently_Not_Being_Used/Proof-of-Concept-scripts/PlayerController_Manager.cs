using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Manager : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    bool alive = true;
    bool birdsEye = false;

    // Use this for initialization
    void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        float ControllerTrigger = Input.GetAxis("PullUpWeapon");

        if (ControllerTrigger == 0.0f)
        {
            playerManagerScript.IsNotGolfing();
            VanillaOrBirdsEyeState();
        }
        else if (ControllerTrigger < 0.0f)
        {
            playerManagerScript.IsNotGolfing();
            WeaponState();
        }
        else if (ControllerTrigger > 0.0f)
        {
            playerManagerScript.IsGolfing();
            GolfingInsideOrOutsideState();
        }

        if (Input.GetKeyDown("joystick button 6"))
        {
            BirdsEyeState();
        }
    }

    //Vanilla and BirdsEye States
    void VanillaOrBirdsEyeState()
    {
        if (birdsEye == false)
        {
            playerManagerScript.VanillaPlayerState();
        }
        else
        {
            playerManagerScript.BirdsEyePlayerState();
        }
    }

    //Weapon State
    void WeaponState()
    {
        playerManagerScript.WeaponPlayerState();
    }

    //Golfing States
    void GolfingInsideOrOutsideState()
    {
        if (playerManagerScript.IsGolfBallHit() == true)
        {
            playerManagerScript.BallFlyingPlayerState();
        }
        else if (playerManagerScript.returnInRing() == false)
        {
            playerManagerScript.GolfingOutsideRingPlayerState();
        }
        else if (playerManagerScript.returnInRing() == true)
        {
            playerManagerScript.GolfingInsideRingPlayerState();
        }
    }


    //BirdsEye Helper Function
    void BirdsEyeState()
    {
        if (birdsEye == false)
        {
            birdsEye = true;
        }
        else
        {
            birdsEye = false;
        }    
    }
}
