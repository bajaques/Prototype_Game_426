using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwing : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    Vector3 startingVector;
    Vector3 rotationVector;

    public Component[] ChildColliders;

    // Use this for initialization
    void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        startingVector = transform.rotation.eulerAngles;
        rotationVector = transform.rotation.eulerAngles;
        rotationVector.y = -90;
        rotationVector.z = 90;

        ChildColliders = GetComponentsInChildren<Collider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Vanilla) && (playerManagerScript.returnSwinging() == false))
        {

            /*foreach (Collider col in ChildColliders)
            {
                col.GetComponent<Collider>().enabled = true;
            }*/

            if (Input.GetKey("joystick button 5"))
            {

                transform.localRotation = Quaternion.Euler(rotationVector);
            }
            else
            {
                
                transform.localRotation = Quaternion.Euler(startingVector);
            }
        }
        else
        {
            /*foreach (Collider col in ChildColliders)
            {
                col.GetComponent<Collider>().enabled = false;
            }*/
        }
    }
}
