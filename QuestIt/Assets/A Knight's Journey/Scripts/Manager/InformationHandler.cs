using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InformationHandler : Singleton<InformationHandler>
{
    public List<PlayerQualities> lobbyPlayers = new List<PlayerQualities>();

    public bool isLoaded = false;

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
    }

    public void SetLobby(List<PlayerQualities> lobby)
    {
        lobbyPlayers = lobby;

        isLoaded = true;
    }
}
