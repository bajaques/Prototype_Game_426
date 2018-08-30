using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMovement : MonoBehaviour 
{
	string b = "buttocks";
	GameObject g;


	// Use this for initialization

	void Start ()
	{
		//print("Hello World");//Hello World 2	
		//PrintNewStatement();
		g = GameObject.Find("Casa");
		MoveAWall();


	}
	
	// Update is called once per frame
	void Update ()
	{
		/*int i = 16677;
		float x = 3.14f;
		bool isworking = true;
		string word = "elephant";
		string words = "horse";



		if (word == "elephant") 
		{
			print(true);
		} 
		else
		{
			print(false);
		}*/

		float x = Input.GetAxis ("Horizontal") * Time.deltaTime * 35.0f;
		float z = Input.GetAxis ("Vertical") * Time.deltaTime * 35.0f;

		transform.Translate (x * 2.0f, 0, z * 2.0f);

	


	}



	void PrintNewStatement()
	{

		print("skins need a superbowl");

		print(b);

	}

	void MoveAWall()
	{
		/*for (int h =0; h<=100; h++)
	
		
		{
			print(h);
			g.transform.Translate(h + 2f,0,0);

		}*/
		//g.transform.Translate(g.transform.position.x + 0.0001f,0,0);
	}



}
