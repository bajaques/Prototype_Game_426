using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEnemySpawner : MonoBehaviour {


    GameObject floatingEnemyCenter;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float minSpawnTime = 4.0f;
    [SerializeField] private float maxSpawnTime = 8.0f;

	// Use this for initialization
	void Start ()
    {
        floatingEnemyCenter = GameObject.Find("FloatingEnemySpawner");
        StartCoroutine(DropEnemies());
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(floatingEnemyCenter.transform.position, Vector3.up, Time.deltaTime * 25.0f);
    }

    IEnumerator DropEnemies()
    {
        float t = Random.Range(minSpawnTime, maxSpawnTime);
        yield return new WaitForSeconds(t);
        Instantiate(EnemyPrefab, transform.position, transform.rotation);
        StartCoroutine(DropEnemies());

    }
}
