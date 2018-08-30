using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDirectionalLight : MonoBehaviour {

    private Color mainColor;
    private Color randomColor;
    private Light componentLight;

    private float t = 0.0f;
    private float t2 = 0.0f;
    private float t3 = 1.0f;
    private bool glowColor = true;
    private bool dayTime = true;

    void Start ()
    {
        randomColor = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        componentLight = GetComponent<Light>();
        mainColor = componentLight.color;
    }
	
	void Update ()
    {

        transform.RotateAround(transform.position, Vector3.up, 2.5f * Time.deltaTime);

        if (glowColor == true)
        {
            if (t < 1)
            {
                t += Time.deltaTime / 10.0f;
                componentLight.color = Color.Lerp(mainColor, randomColor, t);
            }
            else
            {
                glowColor = false;
                t = 0.0f;
            }
        }
        else
        {
            if (t2 < 1)
            {
                t2 += Time.deltaTime / 5.0f;
                componentLight.color = Color.Lerp(randomColor, mainColor, t2);
            }
            else
            {
                randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                glowColor = true;
                t2 = 0.0f;
            }
        }

        if (dayTime == true)
        {
            if (t3 > 0)
            {
                if (t3 > 5.0)
                {
                    t3 -= Time.deltaTime / 100.0f;
                }
                else
                {
                    t3 -= Time.deltaTime / 5.0f;
                }

                //t3 -= Time.deltaTime / 5.0f;
                componentLight.intensity = t3;
            }
            else
            {
                dayTime = false;
            }
        }
        else
        {
            if (t3 < 1.5)
            {
                if (t3 < 5.0f)
                {
                    t3 += Time.deltaTime / 10.0f;
                }
                else
                {
                    t3 += Time.deltaTime / 100.0f;
                }
                componentLight.intensity = t3;
            }
            else
            {
                dayTime = true;
            }
        }
    }
}
