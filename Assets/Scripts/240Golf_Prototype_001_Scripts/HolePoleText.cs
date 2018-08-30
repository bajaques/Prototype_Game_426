using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePoleText : MonoBehaviour {

    public GameObject parentPole;
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(parentPole.transform.position, Vector3.up, 10 * Time.deltaTime);
    }
}
