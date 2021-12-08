using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Renderer displayColourRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayColourUpdated))]
    [SerializeField]
    private Color displayColour = Color.green;

    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        displayColour = newDisplayColour;
    }

        private void HandleDisplayColourUpdated(Color oldColour, Color newColour)
    {
        displayColourRenderer.material.SetColor("_Color", newColour);
    }

}
