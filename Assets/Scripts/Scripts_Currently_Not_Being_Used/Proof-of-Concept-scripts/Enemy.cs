using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject ballRespawnPoint;

    [SerializeField] private GameObject DeathExplosionSet;
    [SerializeField] private GameObject DeathDebrisPrefab;
    [SerializeField] private GameObject EnemyName;
    private GameObject[] DeathDebrisPieces;
    int children;

    private GameObject rotateAround; //this is the object that the thing being shot will rotate around as passed by the OnCollison.
    private bool startSpinning = false;

    private AudioSource[] audios;

    void Start()
    {
        children = DeathExplosionSet.transform.childCount;
        DeathDebrisPieces = new GameObject[children];
        for (int i = 0; i < children; ++i)
        {
            DeathDebrisPieces[i] = DeathDebrisPrefab;
        }

        ballRespawnPoint = GameObject.Find("RespawnHeightPosition");
        if (ballRespawnPoint == null)
        {
           // Destroy(this.gameObject);
        }

        string name = _GM.instance.RandomNameGenerator();
        EnemyName.GetComponent<TextMeshPro>().SetText(name);

        audios = GetComponents<AudioSource>();
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
        //print("here 1"); you're gonna need to figure this out in the collison manager so it doesn't collide with the ground ever.  
        if (col.gameObject.tag == "Bullet")
        {
            rotateAround = col.gameObject;
            startSpinning = true;
            EnemyExplode();
            StartCoroutine(KillEnemyInSeconds());  
        }
        else if (col.gameObject.tag == "MeleeSwipe")
        {
            rotateAround = col.gameObject;
            startSpinning = true;
            EnemyExplode();
            StartCoroutine(KillEnemyInSeconds());
        }
        else if (col.gameObject.tag == "GolfBalls")
        {
            if (ballRespawnPoint == null)
            {
                StartCoroutine(DestroyGolfBall(col));
            }
            else
            {
                StartCoroutine(MoveGolfBall(col));
            }
        }
        else if (col.gameObject.tag == "Shrapnel")
        {
            rotateAround = col.gameObject;
            startSpinning = true;
            EnemyExplode();
            StartCoroutine(KillEnemyInSeconds());
        }
    }

    private void EnemyExplode()
    {
        for (int i = 0; i < children; ++i)
        {
            //Instantiate(DeathDebrisPieces[i], transform.position, DeathDebrisPieces[i].transform.rotation);
        }
    }

    private void ReloadGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator KillEnemyInSeconds()
    {
        audios[Random.Range(0, 3)].Play();
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
    }

    IEnumerator DestroyGolfBall(Collision col)
    {
        yield return new WaitForSeconds(1f);
        Destroy(col.gameObject);
    }

    IEnumerator MoveGolfBall(Collision col)
    {
        yield return new WaitForSeconds(1.0f);
        /*col.gameObject.SetActive(true);
        col.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 5.0f;// this could become predictable???
        col.gameObject.transform.position = ballRespawnPoint.transform.position;*/
        /*Vector3 carryBallPosition = this.gameObject.transform.position;
        carryBallPosition.y = carryBallPosition.y + 5.0f;
        col.gameObject.transform.position = carryBallPosition;
        col.gameObject.transform.parent = this.gameObject.transform;*/

    }


}
