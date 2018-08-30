using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRespawn : MonoBehaviour
{
    public GameObject respawnSpot;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "PlayerCharacter")
        {
            col.gameObject.transform.position = respawnSpot.transform.position;
        }
    }
}
