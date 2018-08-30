using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialDistanceScript : MonoBehaviour {

    public GameObject trialDistanceObject;

	// Use this for initialization
	void Start ()
    {
        float dist = Vector3.Distance(trialDistanceObject.transform.position, transform.position);
        print("TrialDisatnce 1 " + dist);

        float dist2 = Vector3.Distance(transform.position, trialDistanceObject.transform.position);
        print("TrialDisatnce 2 " + dist2);

        float dist3 = Vector3.Distance(transform.position, trialDistanceObject.transform.position);
        print("TrialDisatnce 3 " + dist3);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
