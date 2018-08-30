using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    public bool singlePlayer;
    NavMeshAgent agent;
    GameObject target1;
    GameObject target2;
    private Dictionary<int, GameObject> playerDictionary;


    private int counter = 0;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();

        if (singlePlayer == true)
        {
            target1 = GameObject.Find("GolfBall_Temp_Prefab1");
        }
        else
        {
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                target1 = GameObject.Find("GolfBall_Temp_Prefab1");
            }
            else
            {
                target1 = GameObject.Find("GolfBall_Temp_Prefab2");
            }
        }
       playerDictionary = new Dictionary<int, GameObject>();
       playerDictionary = _GM.instance.GetPlayers();
        if (playerDictionary.Count != 0)
        {
            target1 = playerDictionary[1];//THIS WHOLE CLASS SHOULD BE DEFINED AS TEMP AND MAKESHIFT, WILL CONCENTRATE ON LATER.
        }   
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target1 != null)
        {
            agent.SetDestination(target1.transform.position);
        }
        
    }
}
