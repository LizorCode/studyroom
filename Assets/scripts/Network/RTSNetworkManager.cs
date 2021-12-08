using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class RTSNetworkManager : NetworkManager
{
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientOnConnected?.Invoke();
    }

   public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ClientOnDisconnected?.Invoke();
    }


}
