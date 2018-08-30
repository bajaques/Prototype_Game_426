using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfSwing : MonoBehaviour
{

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    bool poweringSwing = false;
    bool releaseSwing = false;
    float swingPower;
    float swingPowerActual; ////actual recorded power player pushes button for during the golf swing
    int i = 0;


    // Use this for initialization
    void Start()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        poweringSwing = false;
        releaseSwing = false;
        swingPower = 0.0f;
        swingPowerActual = 0.0f; //actual recorded power player pushes button for during the golf swing 
    }

    // Update is called once per frame
   
void Update()
{
    if ((playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Vanilla)
        || (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingOutsideRing))
    {
        if (Input.GetKey("joystick button 0"))
        {
            playerManagerScript.Swinging();
            if (swingPower < 5.0f)
            {
                swingPower = swingPower + 0.1f;
                swingPowerActual = swingPower;
                transform.Rotate(swingPower + 1.0f * Time.deltaTime, 0, 0);
            }
        }

        if (Input.GetKeyUp("joystick button 0"))
        {
            releaseSwing = true;
        }

        if (releaseSwing == true)
        {
            if (swingPower > 0.0f)
            {
                i = i + 1;
                swingPower = swingPower - 0.1f;
                transform.Rotate((-1 * swingPower) - 100.0f * (Time.deltaTime * 2), 0, 0);
            }
            else
            {
                swingPower = 0.0f;
                releaseSwing = false;
                playerManagerScript.NotSwinging();
            }
        }
    }

    else if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.GolfingInsideRing)
    {
        if (Input.GetKey("joystick button 0"))
        {
            playerManagerScript.Swinging();
            if (swingPower < 5.0f)
            {
                swingPower = swingPower + 0.1f;
                swingPowerActual = swingPower;
                transform.Rotate(swingPower + 1.0f * Time.deltaTime, 0, 0);
            }
        }

        if (Input.GetKeyUp("joystick button 0"))
        {
            releaseSwing = true;
        }

        if (releaseSwing == true)
        {
            if (swingPower > 0.0f)
            {
                swingPower = swingPower - 0.1f;
                transform.Rotate((-1 * swingPower) - 100.0f * (Time.deltaTime * 2), 0, 0);
            }
            else
            {
                playerManagerScript.recieveSwingPower(swingPowerActual);
                releaseSwing = false;
                playerManagerScript.NotSwinging();
                swingPower = 0.0f;
            }
        }
    }
}
}

