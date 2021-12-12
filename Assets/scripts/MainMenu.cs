using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RTSNetworkManager networkManager = null;
    [SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        NetworkManager.singleton.StartHost();
    }
}
