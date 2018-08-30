using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEST_XBOX360_controller : MonoBehaviour {

    private Text t;

    // Use this for initialization
    void Start ()
    {
        Transform textTr = transform.Find("Text");
        t = textTr.GetComponent<Text>();
        t.text = "XBOX 360 INPUT TEST: STARTING NOW:";

    }
	
	// Update is called once per frame
	void Update ()
    {
        //float rth = Input.GetAxis("CameraHorizontal") * Time.deltaTime;
        //float rtv = Input.GetAxis("CameraVertical") * Time.deltaTime;
        float rth = Input.GetAxis("CameraHorizontal");
        float rtv = Input.GetAxis("CameraVertical");
        string RightTrigger = rth.ToString();
        string LeftTrigger = rtv.ToString();
        t.text = "RTHorizontal = " + RightTrigger + "\n" + "RTVertical = " + LeftTrigger;
    }
}
