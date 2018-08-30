using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brian_BallScript_Temp : MonoBehaviour {

    GameObject InvisibleWedgeTop;

	// Use this for initialization
	void Start ()
    {
        InvisibleWedgeTop = GameObject.Find("InvisibleWedgeTop");
    }
	
	// Update is called once per frame
	void Update ()
    {
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "InvisibleWedge")
        {
            float hitStrength = collision.gameObject.GetComponent<barley_HingeJoint>().GiveSwingPowerToBall();
            print(hitStrength);
            print("yep we be hittting!!!");
            Vector3 golfBallForward = InvisibleWedgeTop.transform.forward;
            //Vector3 golfBallForward = -transform.right;
            golfBallForward = golfBallForward + new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //GetComponent<Rigidbody>().AddForce((3000.0f * .5f) * golfBallForward);
            //GetComponent<Rigidbody>().AddForce((1500.0f * 5) * golfBallForward);

            GetComponent<Rigidbody>().AddForce(500 * golfBallForward);
        }

    }
   



}
