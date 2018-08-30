using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NEXT TASK - FIND A WAY TO ROTATE FASTER DEPENDING ON HOW LONG THE PLAYER IS PUSHING THE STICK

public class barley_HingeJoint : MonoBehaviour {

    private float reSetPosition = 0.0f;
    private float swingThroughPosition = -29.0f;
    private float pullBackPosition = 150.0F;

    //private float pullBackStrength = 100000.0f;
    private float pullBackStrength = 10000.0f;
    private float pullBackDamper = 1500.0f;
    //private float pullBackDamper = 15.0f;

    private float superHitStrength = 1000000.0f;
    private float flipperDamper = 150.0f;

    private float lowHitStrength = 100.0f;
    [SerializeField] private float flipperDamper2 = 2500.0f;

    HingeJoint hinge;
    JointSpring spring = new JointSpring();
    bool trial = false;

    private float hitStrength = 0.0f; //is passed to ball to show how hard to hit it.

    //GOLF BALL VARIABLES
    GameObject golfBallLocation;
    Vector3 golfBallStartingLocation;
    Quaternion golfBallStartingRotation;
    [SerializeField] private GameObject GolfBallPrefab;

    // Use this for initialization
    void Start ()
    {
        hinge = GetComponent<HingeJoint>();
        spring = new JointSpring();

        golfBallLocation = GameObject.Find("GolfBall_Temp_Prefab");
        golfBallStartingLocation = golfBallLocation.transform.position;
        golfBallStartingRotation = golfBallLocation.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetKey("g"))
        if (Input.GetAxis("CameraVertical") > 0.0f)
        {
            float temp = Input.GetAxis("CameraVertical");
            if (temp > hitStrength)
            {
                hitStrength = temp;
            }
            spring.spring = pullBackStrength;
            spring.damper = pullBackDamper;
            trial = true;
            if (spring.targetPosition != pullBackPosition)
            {
                spring.targetPosition = spring.targetPosition + 10.0f;
            }
            
            hinge.spring = spring;
        }

        //if (Input.GetKeyUp("g"))
        if (Input.GetAxis("CameraVertical") < 0.0f)
        {
            spring.spring = superHitStrength;
            spring.damper = flipperDamper;
            spring.targetPosition = swingThroughPosition;
            hinge.spring = spring;
        }

        if (trial == false)
        {
            spring.spring = lowHitStrength;
            spring.damper = flipperDamper2;
            spring.targetPosition = reSetPosition;
            hinge.spring = spring;
             return;
        }
    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GolfBalls")
        {
            hitStrength = 0.0f;
            trial = false;
            print("hinge joing hit golf ball");
            StartCoroutine(InstantiateANewGolfBall());
        }
    }

    IEnumerator InstantiateANewGolfBall()
    {
        print("we made it.");
        yield return new WaitForSeconds(3);
        GameObject gball = Instantiate(GolfBallPrefab, golfBallStartingLocation, golfBallStartingRotation) as GameObject;
    }

    public float GiveSwingPowerToBall()
    {
        return hitStrength;
    }
}
