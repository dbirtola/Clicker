using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetDiscovery : NetworkDiscovery {
    
    
    public void Start()
    {
        Initialize();
        NetworkTransport.Init();

        //NetworkManager.singleton.networkPort = 7779;
        //NetworkManager.singleton.networkAddress = "localhost";
        //NetworkManager.singleton.StartHost();
    }
   
    public void HostGame()
    {
        NetworkServer.Reset();
        Debug.LogError("Hosting game");
        ConnectionConfig con = new ConnectionConfig();
        var top = new HostTopology(con, 2);


       NetworkTransport.AddHost(top, 7778);
        
        //Switched to this call after Network.player.ipAddress was deprecated. Hasn't been tested yet.
        broadcastData = NetworkManager.singleton.networkAddress; //Network.player.ipAddress;
        StartAsServer();
        
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.networkAddress = NetworkManager.singleton.networkAddress;  //<---- another instance of the new call that hasn't been tested

        NetworkManager.singleton.StartHost();
    }

    public void FindGame()
    {
        ConnectionConfig con = new ConnectionConfig();
        var top = new HostTopology(con, 2);
       NetworkTransport.AddHost(top, 7778);

        StartAsClient();
    }
    
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();
        Debug.LogError("Received broadcast from ip: " + fromAddress);
        NetworkManager.singleton.networkAddress = data;
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartClient();
    }
    
}
