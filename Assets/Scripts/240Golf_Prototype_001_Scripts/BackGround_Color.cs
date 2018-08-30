using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_Color : MonoBehaviour
{

    public Color color0 = Color.red;
    public Color color1 = Color.blue;
    public Color color2 = Color.green;
    public Color color3 = Color.yellow;
    public Color color4 = Color.white;
    public Color color5 = Color.magenta;
    public Color color6 = Color.grey;
    public Color color7 = Color.blue;
    public Color color8 = Color.cyan;
    public Color color9 = Color.black;

    public bool multiColorSky = true;

    private Color[] colors;
    private int r1 = 0;
    private int r2 = 1;



    public float duration = 3.0F;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;

        colors = new Color[10];
        colors[0] = color0;
        colors[1] = color1;
        colors[2] = color2;
        colors[3] = color3;
        colors[4] = color4;
        colors[5] = color5;
        colors[6] = color6;
        colors[7] = color7;
        colors[8] = color8;
        colors[9] = color9;
    }

    void Update()
    {
        if (multiColorSky)
        {
            float t = Mathf.PingPong(Time.time, duration) / duration;

            if (t < 0.15f)
            {
                //Color color = new Color(0.2F, 0.3F, 0.4F);
                Color color1 = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                Color color2 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

                colors[0] = color1;
                colors[1] = color2;

               // r1 = Random.Range(0, 10);
                //r2 = Random.Range(0, 10);
            }
            //print(t);
            //cam.backgroundColor = Color.Lerp(colors[r1], colors[r2], t);
            cam.backgroundColor = Color.Lerp(colors[0], colors[1], t);
        } 
    }
}
