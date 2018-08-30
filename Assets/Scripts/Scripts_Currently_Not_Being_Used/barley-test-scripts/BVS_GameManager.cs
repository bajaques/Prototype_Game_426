using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVS_GameManager : MonoBehaviour
{
    GameObject MannyWalking;
    GameObject MannyGolfing;

    private bool InGolfCircle = false;

	// Use this for initialization
	void Start ()
    {
		MannyGolfing = GameObject.Find("Manny_GolfDrive");
        MannyWalking = GameObject.Find("Walking");
        MannyGolfing.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlayerInGolfCircle(bool b)
    {
        InGolfCircle = b;
        if (b == true)
        {
            Debug.Log("yep we be here");
            MannyWalking.SetActive(false);
            MannyGolfing.SetActive(true);
        }
        else
        {
            //do these things;
            MannyGolfing.SetActive(false);
            MannyWalking.SetActive(true);
            
        }
    }
}
