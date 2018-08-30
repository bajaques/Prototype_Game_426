using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallRing : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

	// Use this for initialization
	void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        //GetComponent<Renderer>().enabled = false;
        //GetComponent<Collider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.BallFlying)
        {
            GetComponent<Behaviour>().enabled = true;
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "SmallHuman_001")
        {
            GetComponent<Behaviour>().enabled = false;
            playerManagerScript.PlayerInRing();

            //gameManagerScript.ShowCurrentPlayerState() == Game_Manager.PlayerStatus.GolfingInsideRing
            //gameManagerScript.GolfingInsideRingPlayerState();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "SmallHuman_001")
        {
            GetComponent<Behaviour>().enabled = true;
            playerManagerScript.PlayerOutRing();
        }
    }


}
