using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    private Light lyte;
    private Color color1;
    private Color color2;
    private float t = 0.0f;

    private bool startSpinning = false;
    private GameObject playerObject = null;
    private Vector3 randomRoll;

    // Use this for initialization
    void Start()
    {
        lyte = GetComponent<Light>();
        color1 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        color2 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        lyte.color = color1;
        GetComponent<Renderer>().material.SetColor("_Color", color1);

        randomRoll = new Vector3(Random.Range(0, 2), Random.Range(0, 2),Random.Range(0, 2));
        //print(randomRoll);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpinning)
        {
            transform.RotateAround(this.gameObject.transform.position, Vector3.up, 500.0f * Time.deltaTime);
            Vector3 playerPlusHeight = new Vector3(0, 1, 0);
            playerPlusHeight = playerPlusHeight + playerObject.transform.position;
            transform.position = Vector3.MoveTowards(this.gameObject.transform.position, playerPlusHeight, 10.0f * Time.deltaTime);
        }
        else
        {
            //transform.RotateAround(this.gameObject.transform.position, Vector3.left, 200.0f * Time.deltaTime);
            transform.RotateAround(this.gameObject.transform.position, randomRoll, 100.0f * Time.deltaTime);
        }

        /*while (t <= 1f)
        {
            lyte.color = Color.Lerp(color1, color2, t);
            t += Time.deltaTime;
            print(t);
        }*/

        //lyte.color = Color.Lerp(color1, color2, Time.deltaTime * 2);
        //lyte.color = Color.LerpUnclamped(color1, color2, Time.deltaTime * 2);

        //color1.

        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, t);

        //lyte.color = Color.Lerp(color1, color2, t//
        if (t < 1)
        { // while t below the end limit...
          // increment it at the desired rate every update:
            t += Time.deltaTime / 10.0f;
            //lyte.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time * .1f, 1.0f));
            //transform.position = Vector3.MoveTowards(this.gameObject.transform.position, transform.position, 5.0f * Time.deltaTime);
        }
        else
        {
            color1 = color2;
            color2 = Random.ColorHSV(0.5f, 1f, 1f, 1f, 0.5f, 1f);
            t = 0.0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerCharacter")
        {
            //print("Here is some more ammo");
            collision.gameObject.GetComponent<Player_Manager_PP_001>().BulletAdder();
            //collision.gameObject.GetComponent<Player_Manager_PP_001>().PlaySound(1);
            Destroy(this.gameObject);
        }
    }

    public void TurnOnAmmo(GameObject go)
    {
        GetComponent<Rigidbody>().useGravity = false;
        startSpinning = true;
        playerObject = go;
    }
}
