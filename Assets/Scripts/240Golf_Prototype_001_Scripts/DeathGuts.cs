using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGuts : MonoBehaviour {

    private AudioSource audio;
    public GameObject TypeOfEnemy2;
    private float x;
    private float y;
    private float z;
    private bool expand;
    private Vector3 rollForward;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (expand)
        {
            x = transform.localScale.x + Time.deltaTime * 5.0f;
            y = transform.localScale.x + Time.deltaTime * 5.0f;
            z = transform.localScale.z + Time.deltaTime * 5.0f;
            Vector3 newLocalScale = new Vector3(x, y, z);
            transform.localScale = newLocalScale;
            transform.position = Vector3.MoveTowards(transform.position, rollForward, Time.deltaTime * 20.0f);
            //transform.position = rollForward;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == ("PlayerCharacter"))
        {
            audio.Play();
            //rollForward = col.gameObject.transform.GetChild(0).transform.forward;
            rollForward = col.gameObject.GetComponent<Player_Manager_PP_001>().playerCam.transform.forward * 10.0f;
            rollForward = rollForward + transform.position;
            //rollForward = rollForward * 10.0f;




            //print(col.gameObject.GetComponent<Player_Manager_PP_001>().playerCam.transform.forward);
            expand = true;
            StartCoroutine(Splatter());
        }
        else if (col.gameObject.tag == ("GolfBalls"))
        {
            //reflect and then splatter.  I want this to expand
        }
        else if (col.gameObject.tag == ("Bullet"))
        {
            //splatter
        }
    }

    IEnumerator Splatter()
    {
        //audio.Play();
        yield return new WaitForSeconds(1);
        Instantiate(TypeOfEnemy2, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
