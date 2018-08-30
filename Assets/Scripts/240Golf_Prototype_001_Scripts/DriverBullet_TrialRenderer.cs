using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverBullet_TrialRenderer : MonoBehaviour {

    private TrailRenderer tr;
    private float RValue;
    private float GValue;
    private float BValue;

    // Use this for initialization
    void Start ()
    {
        float switchColor = Random.Range(0.0f, 3.0f);
        if (switchColor <= 1.0)
        {
            tr = GetComponent<TrailRenderer>();
            RValue = (Random.Range(0.0f, 256.0f) / 255);
            GValue = (Random.Range(0.0f, 256.0f) / 255);
            BValue = (Random.Range(0.0f, 256.0f) / 255);
            tr.startColor = new Color(RValue, GValue, BValue, 1.0f);
        }
    }
}
