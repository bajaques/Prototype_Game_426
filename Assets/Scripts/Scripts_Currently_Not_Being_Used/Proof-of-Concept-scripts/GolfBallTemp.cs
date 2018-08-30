using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallTemp : MonoBehaviour {

    GameObject actualPlayer;

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    bool BallInAir = false;
    bool BallShotIsOver = false;
    bool BallIsPutting = false;

    // Use this for initialization
    void Start ()
    {
        actualPlayer = GameObject.Find("SmallHuman_001"); 
        GetComponent<Behaviour>().enabled = false;

        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (BallIsPutting)
        {
            StartCoroutine(StopThePuttedBall());
        }

        if (BallShotIsOver == true)
        {
            if (GetComponent<Rigidbody>().IsSleeping())
            {
                GetComponent<Rigidbody>().drag = 0;
                GetComponent<Rigidbody>().angularDrag = 0;
                //MAYBE BEFORE THIS WE CAN FIND A RANDOM UMBER TO PLACE IN THE BALL?
                transform.rotation = Quaternion.Euler(0, 90, 0);
                playerManagerScript.GolfBallHasStruckTerrain(false);
                playerManagerScript.changeHitGolfBall(false); // this method returns the camera back to the player after the ball stops moving,maybe
                BallShotIsOver = false;
                float dist = Vector3.Distance(transform.position, actualPlayer.transform.position);
                print("Distance of this Shot is " + dist);
                
            }
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (BallInAir == true)
        {
            if (col.gameObject.tag == "Terrain")
            {
                BallInAir = false;
                playerManagerScript.GolfBallHasStruckTerrain(true);
                StartCoroutine(TurnOffRotation());
                BallShotIsOver = true;
             }   
         }
    }

    
    IEnumerator TurnOffRotation()
    {
        yield return new WaitForSeconds(5);
        GetComponent<Rigidbody>().drag = 5;
        GetComponent<Rigidbody>().angularDrag = 5;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator StopThePuttedBall()
    {
        yield return new WaitForSeconds(5);
        GetComponent<Rigidbody>().drag = 10;
        GetComponent<Rigidbody>().angularDrag = 10;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody>().drag = 0;
        GetComponent<Rigidbody>().angularDrag = 0;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        playerManagerScript.GolfBallHasStruckTerrain(false);
        playerManagerScript.changeHitGolfBall(false); // this method returns the camera back to the player after the ball stops moving,maybe
        BallShotIsOver = false;
        float dist = Vector3.Distance(transform.position, actualPlayer.transform.position);
        print("Distance of this Shot is " + dist);
        BallIsPutting = false;
    }

    public void TurnOnBallLight()
    {
        GetComponent<Behaviour>().enabled = true;
    }

    public void TurnOffBallLight()
    {
        GetComponent<Behaviour>().enabled = false;
    }

    //Actually hits the golf ball by predesignated levels of power as well as the player's swingPower
    public void HitGolfBall(float swingPower)
    {
        if (playerManagerScript.ShowCurrentClub() == Player_Manager.CurrentClub.Driver)
        {
            Vector3 actualPlayerDirection = actualPlayer.transform.forward;
            actualPlayerDirection = actualPlayerDirection + new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().AddForce((300.0f * swingPower) * actualPlayerDirection);
            BallInAir = true;
        }
        else if (playerManagerScript.ShowCurrentClub() == Player_Manager.CurrentClub.Iron)
        {
            Vector3 actualPlayerDirection = actualPlayer.transform.forward;
            actualPlayerDirection = actualPlayerDirection + new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().AddForce((200.0f * swingPower) * actualPlayerDirection);
            BallInAir = true;
        }
        else if (playerManagerScript.ShowCurrentClub() == Player_Manager.CurrentClub.Putter)
        {
            Vector3 actualPlayerDirection = actualPlayer.transform.forward;
            actualPlayerDirection = actualPlayerDirection + new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().AddForce((200.0f * swingPower) * actualPlayerDirection);
            BallIsPutting = true;
        }
    }

    public GameObject GetGolfBall()
    {
        return gameObject;
    }

    public void EnterGolfHole()
    {
        GetComponent<Renderer>().enabled = false;
    }

   
}
