using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Animator_PP_001 : MonoBehaviour {

    Animator anim;
    int crouchBoolHash = Animator.StringToHash("CrouchBool");

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            //anim.SetBool("CrouchBool", true);
            anim.SetBool(crouchBoolHash, true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("CrouchBool", false);
        }
	}
}
