using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barley_Trial_Golf : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("g"))
        {
            GetComponent<Animation>().Play();
        }
        else
        {
           // GetComponent<Animation>().Stop();
        }
        
    }
}
