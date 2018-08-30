using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject ballRespawnPoint;
    public int holeNum;

    //void OnCollisionEnter(Collision col)
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "GolfBalls")
        {
            col.gameObject.SetActive(false);

            //TEMP HARD CODED CODE FOR PLAYTEST
            if (col.gameObject.name == "GolfBall_Temp_Prefab1")
            {
                //this needs to be sent to the gameManager so they can just issue out the commands to all players.  
                GameObject.Find("Player_Prototype_001_002(Clone)").GetComponent<Player_Manager_PP_001>().UpdateGolfHoles(holeNum, 1);
            }
            else if (col.gameObject.name == "GolfBall_Temp_Prefab2")
            {
                GameObject.Find("Player_Prototype_001_002(Clone)").GetComponent<Player_Manager_PP_001>().UpdateGolfHoles(holeNum, 2);
            }


            if (ballRespawnPoint == null)
            {
                StartCoroutine(DestroyGolfBall(col));
            }
            else
            {
                StartCoroutine(MoveGolfBall(col));
            }
        }
        else if (col.gameObject.tag == "PlayerCharacter")
        {
            col.gameObject.GetComponent<Player_Input_Manager_PP_001>().overheadMap = true;
            _GM.instance.CurrentHolePlayerTouched(this.gameObject);
        }
    }

    IEnumerator DestroyGolfBall(Collider col)
    {
        yield return new WaitForSeconds(1f);
        Destroy(col.gameObject);
    }

    IEnumerator MoveGolfBall(Collider col)
    {
        yield return new WaitForSeconds(1.0f);
        col.gameObject.SetActive(true);
        col.gameObject.transform.position = _GM.instance.ballrespawnPoint.position;
    }
}
