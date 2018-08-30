using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//USED TO SEE IF A PLAYER IS COLLIDING WITH THE CIRCLE PLATFORM THAT CIRCLES THE GOLF BALL, AND GOES ITO GOLF VIEW
public class GolfBall_CirclePlatform_Collision : MonoBehaviour {

    GameObject GameManager;
    BVS_GameManager gameManageScript;

    // Use this for initialization
    void Start ()
    {
        GameManager = GameObject.Find("Terrain_001");
        gameManageScript = GameManager.GetComponent<BVS_GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerCharacter")
        {
            Debug.Log("Character Enters");
            gameManageScript.PlayerInGolfCircle(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "PlayerCharacter")
        {
            Debug.Log("Character Exits");
            gameManageScript.PlayerInGolfCircle(false);
        }
    }

}
