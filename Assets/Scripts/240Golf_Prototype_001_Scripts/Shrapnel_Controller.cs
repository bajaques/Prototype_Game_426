using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel_Controller : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SelfDestruct());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator SelfDestruct()
    {
        float rand = Random.Range(10.0f, 21.0f);
        yield return new WaitForSeconds(rand);
        Destroy(this.gameObject);
    }
}
