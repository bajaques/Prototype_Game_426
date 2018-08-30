using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Putter_CirclePlayer : MonoBehaviour {

    private GameObject playerToFollow = null;
    private int randomRotate;
    private int randomGravity;
    private bool spin = false;



    // Use this for initialization
    void Start ()
    {
        randomRotate = Random.Range(0, 2);
        randomGravity = Random.Range(0, 4);
        if (randomGravity == 0)
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (spin)
        {
            if (randomRotate == 0)
            {
                transform.RotateAround(playerToFollow.transform.position, Vector3.up, 200.0f * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(playerToFollow.transform.position, -Vector3.up, 200.0f * Time.deltaTime);
            }
        }
    }

    public void SetPlayerToFollow(GameObject player)
    {
        playerToFollow = player;
        StartCoroutine(StartRotating());
    }

    IEnumerator StartRotating()
    {
        yield return new WaitForSeconds(0.2f);
        spin = true;
    }
}
