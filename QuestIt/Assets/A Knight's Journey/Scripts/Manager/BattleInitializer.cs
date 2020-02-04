﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInitializer : Singleton<BattleInitializer>
{
    public GameObject lobbyUI;
    public List<PlayerQualities> lobbyPlayers = new List<PlayerQualities>();

    /// <summary>Adds a Battle Player to the Lobby. </summary>
    /// <param name="player"> Battle Player</param>

    protected override void Awake()
    {
        base.Awake();

        isDontDestroyOnLoad = true;
    }

    public void AddPlayer(bool isTeamRed)
    {
        PlayerQualities quality = new PlayerQualities();

        quality.character = BattleCharacters.BESTOFWORLDS;

        quality.IsTeamRed = isTeamRed;

        for(int i =1;i<=4;i++)
        {
            quality.chosenMoves.Add((MOVES)i);
        }

        quality.myAttributes = PlayerGenerator.AttributesGenerator();


        AddaBattlePlayer(quality);
    }

    public void StartLobby()
    {
        if( !InitializeLobby())
        {
            Debug.Log("Something failed");
        }
    }

    public void AddaBattlePlayer(PlayerQualities player)
    {
        if (lobbyPlayers == null)
        {
            lobbyPlayers = new List<PlayerQualities>();
        }

        lobbyPlayers.Add(player);
    }

    public bool InitializeLobby()
    { 
        if (lobbyPlayers.Count > 1)
        {
            lobbyPlayers[0].IsPlayer = true;

            lobbyUI.gameObject.SetActive(false);

            LoadManager.Instance.LoadBattleScene(lobbyPlayers, "BattleGround");

            return true;
        }

        return false;
    }
}