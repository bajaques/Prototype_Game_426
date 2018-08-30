using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager_Test : MonoBehaviour {


    private const string typeName = "240Golf";
    private const string gameName = "GoldenBallsPuttPutt";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
        MasterServer.ipAddress = "127.0.0.1";
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            float w, h;
            w = h = 800;
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect((Screen.width - w) / 2, (Screen.height - h) / 2,w,h), "Start Server"))
            {
                StartServer();
            }
        }
    }
}
