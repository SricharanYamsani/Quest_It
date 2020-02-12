using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InformationHandler : Singleton<InformationHandler>
{
    public List<PlayerInfo> lobbyPlayers = new List<PlayerInfo>();

    public bool isLoaded = false;

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
    }

    public void SetLobby(List<PlayerInfo> lobby)
    {
        lobbyPlayers = lobby;

        isLoaded = true;
    }
}
