using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisbleWdgeTop : MonoBehaviour {

    GameObject golfBallLocation;
    Vector3 golfBallStartingLocation;

    [SerializeField] private float speedUpRotation = 25.0f;

    // Use this for initialization
    void Start ()
    {
        golfBallLocation = GameObject.Find("GolfBall_Temp_Prefab");
        golfBallStartingLocation = golfBallLocation.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float rot = Input.GetAxis("Horizontal");
        
        if (rot != 0)
        {
            speedUpRotation = speedUpRotation + 0.5f;
            transform.RotateAround(golfBallStartingLocation, Vector3.up, -rot  * Time.deltaTime * speedUpRotation);
            //print("SpeedUp " + speedUpRotation);
        }
        else
        {
            speedUpRotation = 25.0f;
            //print("Slow Down " + speedUpRotation);
        }
    }
}
