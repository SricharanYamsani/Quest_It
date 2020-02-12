using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInitializer : Singleton<BattleInitializer>
{
    public GameObject lobbyUI;
    public List<PlayerInfo> lobbyPlayers = new List<PlayerInfo>();

    /// <summary>Adds a Battle Player to the Lobby. </summary>
    /// <param name="player"> Battle Player</param>

    protected override void Awake()
    {
        base.Awake();

        isDontDestroyOnLoad = true;
    }

    public void AddPlayer(bool isTeamRed)
    {
        PlayerInfo quality = new PlayerInfo();

        quality.character = BattleCharacters.WIZARD;

        quality.IsTeamRed = isTeamRed;

        for (int i = 1; i <= 4; i++)
        {
            quality.chosenMoves.Add((Moves)i);
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

    public void AddaBattlePlayer(PlayerInfo player)
    {
        if (lobbyPlayers == null)
        {
            lobbyPlayers = new List<PlayerInfo>();
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
