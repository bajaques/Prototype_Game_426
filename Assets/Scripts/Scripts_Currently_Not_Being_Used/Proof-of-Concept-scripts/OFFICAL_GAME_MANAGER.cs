using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//this class and script needs to be renamed unless we actually make it the official game manager
public class OFFICAL_GAME_MANAGER : NetworkBehaviour {



    //I want this offcial game manager script to eventually be the one stop all for anything we want the game to be able to turn off or on in the start menu
    //examples being, turn on enemies, turn on friendly fire, ect.  

    //Enemies
    public GameObject TypeOfEnemy1;
    public GameObject TypeOfEnemy2;
    public Transform EnemySpawn;
    public Transform EnemySpawn1;
    public Transform EnemySpawn2;
    public Transform EnemySpawn3;
    public Transform EnemySpawn4;
    public bool TurnOnEnemies; //turns on or off, enemies respawning;
    public bool TurnOnGrassHoppers;
    public bool TurnOnBigEvilJohns;
    public float spawnRate = 10.0f;

    public List<GameObject> allEnemies = new List<GameObject>();

	
	// Update is called once per frame
	void Update ()
    {
        if (TurnOnEnemies == true)
        {
            TurnOnEnemiesFunction();
        }
    }


    void TurnOnEnemiesFunction()
    {
        TurnOnEnemies = false;
        StartCoroutine(EnemiesAreCreated());
    }

    IEnumerator EnemiesAreCreated()
    {
        yield return new WaitForSeconds(spawnRate);
        CmdInstantiateEnemies();
        TurnOnEnemies = true;
    }


    //[Command]
    void CmdInstantiateEnemies()
    {
        if (isServer)
        {
            if (TurnOnBigEvilJohns == true)
            {
                var Enemy = (GameObject)Instantiate(TypeOfEnemy1, EnemySpawn.position, EnemySpawn.rotation);
            }
            if (TurnOnGrassHoppers == true)
            {
                var grassHopperEnemy1 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn1.position, EnemySpawn1.rotation);
                var grassHopperEnemy2 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn2.position, EnemySpawn2.rotation);
                var grassHopperEnemy3 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn3.position, EnemySpawn3.rotation);
                var grassHopperEnemy4 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn4.position, EnemySpawn4.rotation);
            }
            RpcInstantiateEnemies();
        }
    }

    [ClientRpc]
    void RpcInstantiateEnemies()
    {
        if (!isServer)
        {
            if (TurnOnBigEvilJohns == true)
            {
                var Enemy = (GameObject)Instantiate(TypeOfEnemy1, EnemySpawn.position, EnemySpawn.rotation);
            }
            if (TurnOnGrassHoppers == true)
            {
                var grassHopperEnemy1 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn1.position, EnemySpawn1.rotation);
                var grassHopperEnemy2 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn2.position, EnemySpawn2.rotation);
                var grassHopperEnemy3 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn3.position, EnemySpawn3.rotation);
                var grassHopperEnemy4 = (GameObject)Instantiate(TypeOfEnemy2, EnemySpawn4.position, EnemySpawn4.rotation);
            }
        }
    }



}
