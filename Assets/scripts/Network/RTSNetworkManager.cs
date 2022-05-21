using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    // [Header("Game")]
    [SerializeField] private GameObject gamePlayerPrefab = null;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;

    public List<RTSPlayer> Players { get; } = new List<RTSPlayer>();
    // public List <GameObject> GamePlayers { get; } = new List<GameObject>();

    #region Server

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!isGameInProgress) { return; }

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }

    public void StartGame()
    {
        if (Players.Count < 2) { return; }

        isGameInProgress = true;

        ServerChangeScene("First");
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().name.StartsWith("First"))
        {   
            Debug.Log("work1");
            for (int i = Players.Count - 1; i >= 0; i--)
            {
                var conn = Players[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                
                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        Players.Add(player);

        player.SetDisplayName($"Player {Players.Count}");


        player.SetPartyOwner(Players.Count == 1);
    }



    #endregion

    #region Client

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

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion
}
