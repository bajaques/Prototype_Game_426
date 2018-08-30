using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barleyPlayerCameraController : MonoBehaviour {

   
    GameObject barleyPlayerController;
    barleyPlayerController barleyPlayerControllerScript;



    // Use this for initialization
    void Start ()
    {
        barleyPlayerController = GameObject.Find("Walking");
        barleyPlayerControllerScript = barleyPlayerController.GetComponent<barleyPlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
