using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField addressInput = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join() //подключение клиента к серверу
    {
        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected() 
    {
        joinButton.interactable = true; //можно использовать кнопку присоединиться

        gameObject.SetActive(false); //сделать кнопку активной
        landingPagePanel.SetActive(false); //деактивировать основную форму
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true; //можно использовать кнопку присоединиться
    }
}
