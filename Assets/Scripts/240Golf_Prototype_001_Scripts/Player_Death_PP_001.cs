using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Death_PP_001 : NetworkBehaviour {

    [SerializeField] private Player_Input_Manager_PP_001 playerInputManagerScript;

    [SerializeField] private GameObject DeathExplosionSet;
    [SerializeField] private GameObject DeathDebrisPrefab;
    private GameObject[] DeathDebrisPieces;
    private NetworkStartPosition[] spawnPoints;
    
    private GameObject rotateAround; //this is the object that the thing being shot will rotate around as passed by the OnCollison.
    private bool startSpinning = false; //now that the thing is dying it will start spinning in update function.  
    public bool debugDeath;
    private int children;

    // Use this for initialization
    void Start ()
    {
        children = DeathExplosionSet.transform.childCount;
        DeathDebrisPieces = new GameObject[children];
        for (int i = 0; i < children; ++i)
        {
            DeathDebrisPieces[i] = DeathDebrisPrefab;
        }

        spawnPoints = FindObjectsOfType<NetworkStartPosition>();

    }

    void Update()
    {
        if (startSpinning == true)
        {
            transform.RotateAround(transform.position, transform.up, 1000 * Time.deltaTime);
        }  
    }
	
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            if (debugDeath == true)
            {
                rotateAround = col.gameObject;
                startSpinning = true;
                PlayerExplode();
                StartCoroutine(PlayerDie());
            }
            else
            {
                playerInputManagerScript.SetAliveStatus(false);
            }
        }
    }

    void PlayerExplode()
    {
        for (int i = 0; i < children; ++i)
        {
            //Instantiate(DeathDebrisPieces[i], transform.position, DeathDebrisPieces[i].transform.rotation);
        }
    }


    IEnumerator PlayerDie()
    {
        int random = UnityEngine.Random.Range(1, 2);
        yield return new WaitForSeconds(random);
        //yield return new WaitForSeconds(2.0f);
        //Destroy(this.gameObject);
        startSpinning = false;
        RpcRespawn();

    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }




}
