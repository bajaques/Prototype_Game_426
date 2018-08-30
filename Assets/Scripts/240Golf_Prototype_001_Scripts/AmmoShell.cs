using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShell : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "PlayerCharacter")
        {
            transform.parent.GetComponent<Ammo>().TurnOnAmmo(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
