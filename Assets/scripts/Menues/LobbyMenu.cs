using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];

    [SyncVar,SerializeField] private bool sg;
    [SerializeField] private GameObject lobbyPanel;

    private void Start()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        RTSPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    private void OnDestroy()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        RTSPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void Update() 

    {
        if (sg)
        {
            lobbyPanel.SetActive(false);
            foreach (RTSPlayer player in ((RTSNetworkManager)NetworkManager.singleton).Players)
            {
                player.StartGame();
            }
        }

    }

    private void HandleClientConnected() //активировать форму комнаты ожидания
    {
        lobbyUI.SetActive(true);
    }

    private void ClientHandleInfoUpdated() //обновление текстовых полей в комнате ожидания
    {
        List<RTSPlayer> players = ((RTSNetworkManager)NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].GetDisplayName();
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for student...";
        }

        startGameButton.interactable = players.Count >= 2; //активировать кнопку старт, если пользователей больше 2
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state) //активация кнопки старт
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void StartGame() //переход в 3D режим
    {
        foreach(RTSPlayer player in ((RTSNetworkManager)NetworkManager.singleton).Players) 
        {
            player.StartGame();
        }
        sg = true;
    }

    public void LeaveLobby() // вернуться в главное меню
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
        
        SceneManager.LoadScene(0);
        
    }
}
