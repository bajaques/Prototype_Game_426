using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.networkAddress = "192.168.0.11";
        NetworkManager.singleton.StartClient();
    }

    /*void Start()
    {
        NetworkManager.singleton.networkAddress = "192.168.0.11";
        NetworkManager.singleton.StartClient();
    }*/

}
