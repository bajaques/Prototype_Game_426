#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

	public class NetworkManagerHUD : MonoBehaviour
	{
        // Runtime variable
        bool showServer = false;
        private float widthSize = 300;
        private bool logoDraw = true;
        private bool startScreen = false;

        public NetworkManager manager;
        public Network n;
		[SerializeField] public bool showGUI = true;
        private int offsetX = (Screen.width - 300) / 2;
        private int offsetY = (Screen.height - 40) / 2;

        public Texture aTexture;

        void Awake()
		{
			manager = GetComponent<NetworkManager>();
        }

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

            int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 44;

            if (logoDraw)
            {
                GUI.DrawTexture(new Rect((Screen.width - (Screen.width / 2)) / 2, 0, Screen.width / 2, Screen.height / 2), aTexture);
            }

            GUI.color = Color.yellow;
            GUI.backgroundColor = Color.green;
            GUIStyle myStyle = new GUIStyle(GUI.skin.button);
            myStyle.fontSize = 30;
            myStyle.onHover.textColor = Color.red;
            Font myFont = (Font)Resources.Load("Fonts/InsertFun", typeof(Font));
            myStyle.font = myFont;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, widthSize, 40), "LAN Host(H)", myStyle))
				{ 
                    manager.StartHost();
                }
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 155, 40), "LAN Client(C)", myStyle))
				{
					manager.StartClient();
                }
				manager.networkAddress = GUI.TextField(new Rect(xpos + 155, ypos, 145, 40), manager.networkAddress, myStyle);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, widthSize, 40), "LAN Server Only(S)", myStyle))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
                logoDraw = false;
                //Toggle the Start Menu
                if (Input.GetKeyDown("joystick button 7"))
                {
                    print("Helllo are we actually printing anything????");
                    if (startScreen == false)
                    {
                        startScreen = true;
                    }
                    else
                    {
                        startScreen = false;
                    }
                }

                if (startScreen)//
                {//
                    if (NetworkServer.active)
                    {
                        logoDraw = false;
                        GUI.Label(new Rect(0, 0, widthSize + 50, 40), "Server: port=" + manager.networkPort, myStyle);
                        ypos = spacing;
                    }
                    if (NetworkClient.active)
                    {
                        if (NetworkServer.active)
                        {
                            logoDraw = false;
                            GUI.Label(new Rect(0, ypos, widthSize + 50, 40), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort, myStyle);
                            ypos += spacing;
                        }
                        else
                        {
                            logoDraw = false;
                            GUI.Label(new Rect(0, 0, widthSize + 50, 40), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort, myStyle);
                            ypos = spacing;
                        }
                    }
                }//
			}

            if (startScreen)
            {
                if (NetworkClient.active && !ClientScene.ready)
                {
                    if (GUI.Button(new Rect(0, ypos, widthSize + 50, 40), "Client Ready", myStyle))
                    {
                        ClientScene.Ready(manager.client.connection);

                        if (ClientScene.localPlayers.Count == 0)
                        {
                            ClientScene.AddPlayer(0);
                        }
                    }
                    ypos += spacing;
                }

                if (NetworkServer.active || NetworkClient.active)
                {
                    if (GUI.Button(new Rect(0, ypos, widthSize + 50, 40), "Stop (X)", myStyle))
                    {
                        logoDraw = true;
                        manager.StopHost();
                    }
                    ypos += spacing;
                }
            }
               
			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, widthSize, 40), "Enable Match Maker (M)", myStyle))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Create Internet Match", myStyle))
							{
                                manager.matchMaker.CreateMatch("roomName", 4, true, "", "", "", 0, 0, manager.OnMatchCreate);
                            }
							ypos += spacing;

							GUI.Label(new Rect(0, ypos, 155, 40), "Room Name:", myStyle);
							manager.matchName = GUI.TextField(new Rect(0+155, ypos, 145, 40), manager.matchName, myStyle);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Find Internet Match", myStyle))
							{
                                manager.matchMaker.ListMatches(0, 10, "", true, 0, 0, manager.OnMatchList);
                            }
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Join Match:" + match.name, myStyle))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                }
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Change MM server", myStyle))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Local", myStyle))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(0, ypos, 100, 40), "Internet", myStyle))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(0, ypos, 100, 40), "Staging", myStyle))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

					GUI.Label(new Rect(0, ypos, widthSize + 50, 40), "MM Uri: " + manager.matchMaker.baseUri, myStyle);
					ypos += spacing;

					if (GUI.Button(new Rect(0, ypos, widthSize, 40), "Disable Match Maker", myStyle))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}
	}
};
#endif //ENABLE_UNET
