using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_LargeHuman : MonoBehaviour {

    public GameObject Waypoints;
    public GameObject NPCLargeMesh;
    public BoxCollider boxCollider1;
    public BoxCollider boxCollider2;
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private Transform target;
    private int targetNumber = 0;
    private float nextActionTime = 0.0f;
    private float period = 0.1f;
    private float timeCounter = 120.0f;
    private bool switchNPC = false;

    // Use this for initialization
    void Start ()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        NPCLargeMesh.gameObject.GetComponent<Renderer>().enabled = false;
        boxCollider1.enabled = false;
        boxCollider2.enabled = false;
        agent = GetComponent<NavMeshAgent>();
        waypoints = Waypoints.GetComponentsInChildren<Transform>();
        /*for (int i = 1; i < waypoints.Length; ++i)
        {
            print(waypoints[i].name);
        }*/
        target = waypoints[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter > nextActionTime)
        {
            //print(nextActionTime);
            nextActionTime += period;
            return;
        }
        else
        {
            nextActionTime = 0.0f;
            if (switchNPC == false)
            {
                timeCounter = 30.0f;
                switchNPC = true;
                gameObject.GetComponent<Animator>().enabled = true;
                NPCLargeMesh.gameObject.GetComponent<Renderer>().enabled = true;
                boxCollider1.enabled = true;
                boxCollider2.enabled = true;
            }
            else
            {
                timeCounter = 120.0f;
                switchNPC = false;
                gameObject.GetComponent<Animator>().enabled = false;
                NPCLargeMesh.gameObject.GetComponent<Renderer>().enabled = false;
                boxCollider1.enabled = false;
                boxCollider2.enabled = false;
            }
        }

        if (switchNPC)
        {
            RunDestinationPath();
        }
    }

    private void RunDestinationPath()
    {
      agent.SetDestination(target.position);
      if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance)
      {
          if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
          {
              if (targetNumber < waypoints.Length)
              {
                  targetNumber = targetNumber + 1;
                  target = waypoints[targetNumber].transform;
              }
              else
              {
                  targetNumber = 1;
                  target = waypoints[targetNumber].transform;
              }
          }
        }
    }
}


