using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {


    GameObject golfBallTemp;
    GolfBallTemp golfBallTempScript;

    public bool isInRing = false;
    public bool isGolfing = false;
    public bool hitGolfBall = false;
    public bool isSwinging = false;
    public bool GolfBallStruckTerrain = false;

   

    public enum PlayerStatus { Vanilla, Weapon, GolfingOutsideRing, GolfingInsideRing, BallFlying, BirdsEye, Dead };
    PlayerStatus playerCurrentState;

    public enum CurrentClub { Driver, Iron, Putter };
    CurrentClub clubSelected;

    // Use this for initialization
    void Start ()
    {
        golfBallTemp = GameObject.Find("GolfBall_Temp");
        golfBallTempScript = golfBallTemp.GetComponent<GolfBallTemp>();

        isInRing = false;
        isGolfing = false;
        hitGolfBall = false;
        isSwinging = false;
        GolfBallStruckTerrain = false;

        playerCurrentState = PlayerStatus.Vanilla;
        clubSelected = CurrentClub.Driver;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((isGolfing == true) && (isInRing == false))
        {
            golfBallTempScript.TurnOffBallLight();
            GolfingOutsideRingPlayerState();
        }
        else if ((isGolfing == true) && (isInRing == true))
        {
            golfBallTempScript.TurnOnBallLight();
            GolfingInsideRingPlayerState();
        }
        else if ((isInRing == true) && (isGolfing == false))
        {
            golfBallTempScript.TurnOffBallLight();
        }
        else if ((isInRing == false) && (isGolfing == false))
        {
            golfBallTempScript.TurnOffBallLight();
        }
	}

    public void recieveSwingPower(float swingPower)
    {
        if ((isInRing == true) && (isGolfing == true))
        {
            hitGolfBall = true;
            golfBallTempScript.HitGolfBall(swingPower);
        }
    }


    public void Swinging()
    {
        isSwinging = true;
    }

    public void NotSwinging()
    {
        isSwinging = false;
    }

    public bool returnSwinging()
    {
        return isSwinging;
    }

    public void PlayerInRing()
    {
        isInRing = true;
    }

    public void PlayerOutRing()
    {
        isInRing = false;
    }
    public bool returnInRing()
    {
        return isInRing;
    }

    public void IsGolfing()
    {
        isGolfing = true;
    }

    public void IsNotGolfing()
    {
        isGolfing = false;
    }

    public bool IsGolfBallHit()
    {
        return hitGolfBall;
    }

    public void changeHitGolfBall(bool b)
    {
        hitGolfBall = b;
        return;
    }


    //this accesses if a golfball has hit the earth after it has been shot so the camera knows to start spinning around it.
    public void GolfBallHasStruckTerrain(bool b)
    {
        GolfBallStruckTerrain = b;
        return;
    }

    public bool ReturnGolfBallHasStruckTerrain()
    {
        return GolfBallStruckTerrain;
    }

    //GolfBall has hit a Golf Hole
    public void EnterGolfHole()
    {
        golfBallTempScript.EnterGolfHole();
    }


    //The Current Player States
    //Playerstates
    /*1.)  Vanilla - Ability to melee with all golf clubs and swing golf clubs but not hit ball - Default
     * 2.) Weapon - Switches clubs to be weapons (holding down Left Trigger (360) and can fire his cubs using Right Bumber(360)
     * 3.) Golfing - Player is in radius of golf ball, holding Right Trigger, and can hit a golf ball
     * 4.) BallFlying - Ball is travelling after being hit, player has ability to help guide ball in air
     * 5.) BirdsEye - Player switches to overhead map and can find ball on disovered portions of map, maybe even fast travel
     * 6.) Dead - Player has died and is in process of respawning?
     * */
    public PlayerStatus ShowCurrentPlayerState()
    {
        return playerCurrentState;
    }

    public void VanillaPlayerState()
    {
        playerCurrentState = PlayerStatus.Vanilla;
    }

    public void WeaponPlayerState()
    {
        playerCurrentState = PlayerStatus.Weapon;
    }

    public void GolfingOutsideRingPlayerState()
    {
        playerCurrentState = PlayerStatus.GolfingOutsideRing;  
    }

    public void GolfingInsideRingPlayerState()
    {
        playerCurrentState = PlayerStatus.GolfingInsideRing;
    }

    public void BallFlyingPlayerState()
    {
        playerCurrentState = PlayerStatus.BallFlying;
    }

    public void BirdsEyePlayerState()
    {
        playerCurrentState = PlayerStatus.BirdsEye;
    }

    public void DeadPlayerState()
    {
        playerCurrentState = PlayerStatus.Dead;
    }



    //CurrentClub Getter and Setter
    public void CurrentClubIsDriver()
    {
        clubSelected = CurrentClub.Driver;
    }

    public void CurrentClubIsIron()
    {
        clubSelected = CurrentClub.Iron;
    }

    public void CurrentClubIsPutter()
    {
        clubSelected = CurrentClub.Putter;
    }

    public CurrentClub ShowCurrentClub()
    {
        return clubSelected;
    }



}
