using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallScript : MonoBehaviour {


	Vector3 StartingPosition;
	Vector3 EndingPosition; 

	[SerializeField]
	private float Speed = 1.0f;

	// Use this for initialization
	void Start () 
	{
		StartingPosition = transform.position; 	 
		print (StartingPosition.x + " the x starting position "); 
		print (StartingPosition.y);
		print (StartingPosition.z); 
		print (StartingPosition);
	
		EndingPosition = new Vector3 (280.9f, 31f, 228f);
		print (EndingPosition.x); 
		print (EndingPosition.y);
		print (EndingPosition.z); 
		print (EndingPosition);

		Speed = 1.0f;
	
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		

		transform.position = new Vector3(Mathf.PingPong(Time.time * Speed, EndingPosition.x - StartingPosition.x) + StartingPosition.x
		, StartingPosition.y, 
		Mathf.PingPong(Time.time * Speed, EndingPosition.z - StartingPosition.z) + StartingPosition.z);
	

	
	
	
	
	}
}
