using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainCameraBehaviour : MonoBehaviour
{

    public Transform centerOfWorld;
    public float speed = 20.0f;
    private float cloudSpeed = 500.0f;
    public bool randomRotate;
    private int rotateValue = 0;
    private bool spin = true;
    private bool charSelect = false;
    private int hitBy = 0; //who the cloud was hit by
    private Vector3 moveVec;

	// Use this for initialization
	void Start ()
    {

        if (centerOfWorld == null)
        {
            Destroy(this.gameObject);
            print("CenterOfWorld is null from either the beginning main camera or the character select clouds.");
        }
        if (randomRotate)
        {
            rotateValue = Random.Range(0, 2);
        }  
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (spin)
        {
            if (rotateValue == 0) transform.RotateAround(centerOfWorld.position, Vector3.up, speed * Time.deltaTime);
            else transform.RotateAround(-centerOfWorld.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up, cloudSpeed * Time.deltaTime);
        } 

        if (charSelect)
        {
            float step = 20.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveVec, step);

            if (transform.position == moveVec)
            {
                charSelect = false;
                cloudSpeed = 50.0f;
                _GM.instance.GetPlayer(hitBy).GetComponent<Player_TEMP_Input_Manager_PP_001>().UpdateState2();
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "GolfBalls")
        {
            HitByGolfBall(col);
        }
        else if (col.gameObject.tag == "Bullet")
        {
            HitByBullet(col);
        }
    }

    private void HitByGolfBall(Collision col)
    {
        if (charSelect) // this needs to be from the game Manager perhaps?
        {
            //So this is interesting because we could get a Mario Party type of thing here where multiple people can steal each other's cloud
            //or the random bounces can chance due to the one you were aiming at, which I kind of like better to be honest.  So let's think about that. 
            //how do we design for that type of fun beginning???
            //Myabe all the players start off on the same picnic table and then are given a few moments to decide what to do next.   
            Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
            return;
        }

        int i = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        foreach (GameObject child in allChildren)
        {
            child.layer = 0;
        }

        spin = false;
        hitBy = col.gameObject.GetComponent<IGolfable>().lastHitBy;
        _GM.instance.GetPlayer(hitBy).GetComponent<Player_TEMP_Input_Manager_PP_001>().UpdateState();//this will be moved till after the cloud has make its movement

        Vector3 t = _GM.instance.GetPlayer(hitBy).GetComponent<Player_TEMP_Input_Manager_PP_001>().GetStartTransform();
        t = t + new Vector3(25, 0, 0);
        Vector3 u = transform.position - t;
        u.Normalize();
        float dist = Vector3.Distance(t, transform.position);
        dist = dist / 25.0f;
        u = u * dist;
        moveVec = t + u;
        charSelect = true;
        StartCoroutine(SpinCharSelectQuickly());
    }

    private void HitByBullet(Collision col)
    {
        //get the color of teh object and give it to the player in their player struct and change their color
        //do something fancy to show that it is being hit 
        //change the proper states of the tutorial and what nots
        //destroy the clouds
        //set Manny on his merry way by deactibating and activating all the proper scripts
        _GM.instance.GetPlayer(hitBy).GetComponent<Player_TEMP_Input_Manager_PP_001>().UpdateState3(transform.GetChild(3).GetComponent<TextMeshPro>().text,
            transform.GetChild(2).GetComponent<Renderer>().material.color);
        Destroy(this.gameObject);
    }

    IEnumerator SpinCharSelectQuickly()
    {
        yield return new WaitForSeconds(2.0f);
        //grab the cloud and center the message on the player's screen.
        //Destroy(this.gameObject);
    }
}
