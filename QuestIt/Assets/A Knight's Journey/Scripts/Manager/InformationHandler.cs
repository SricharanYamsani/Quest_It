using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InformationHandler : Singleton<InformationHandler>
{
    public List<PlayerInfo> lobbyPlayers;

    public bool isLoaded = false;

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
    }

    public void SetLobby(List<PlayerInfo> lobby)
    {
        if (lobbyPlayers == null)
        {
            lobbyPlayers = new List<PlayerInfo>();
        }
        lobbyPlayers = lobby;

        isLoaded = true;

        Logger.Log("Lobby Set");
    }
}
