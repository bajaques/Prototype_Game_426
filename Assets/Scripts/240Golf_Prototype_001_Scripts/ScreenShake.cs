using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public float shakeDuration;
    private float constShakeDuration;
    public float shakeAmount;
    public float decreaseFactor;
    private bool gunFireShake = false;
    private Vector3 originalPos;

    void Start()
    {
        constShakeDuration = shakeDuration;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (gunFireShake)
        {
            if (shakeDuration > 0)
            {
                transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                gunFireShake = false;
                shakeDuration = constShakeDuration;
                transform.position = originalPos;  
            }
        }
	}

    public void GunFireShake()
    {
        originalPos = transform.position; //might need to be localPosition;
        gunFireShake = true;
    }
}
