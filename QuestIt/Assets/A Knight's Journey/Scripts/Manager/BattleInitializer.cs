using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleInitializer : Singleton<BattleInitializer>
{
    public List<PlayerInfo> lobbyPlayers;

    /// <summary>Adds a Battle Player to the Lobby. </summary>
    /// <param name="player"> Battle Player</param>

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();

    }

    public void AddPlayer(bool isTeamRed)
    {
        PlayerInfo quality = new PlayerInfo();

        quality.character = BattleCharacters.OCCULTIST;

        quality.IsTeamRed = isTeamRed;

        quality.chosenMoves.Add(Moves.GUT_MID_1);
        quality.chosenMoves.Add(Moves.PIERCE_ATTACK_1);
        quality.chosenMoves.Add(Moves.MAGIC_HEAL_SMALL_1);
        quality.chosenMoves.Add(Moves.LIGHTNING_SMALL_1);

        quality.myAttributes = PlayerGenerator.AttributesGenerator();


        AddBattlePlayer(quality);
    }

    public void StartLobby()
    {
        if (!InitializeLobby())
        {
            Debug.Log("Something failed");
        }
    }

    public void AddBattlePlayer(PlayerInfo player)
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
            LoadManager.Instance.LoadBattleScene(lobbyPlayers, "BattleGround");

            return true;
        }

        return false;
    }

    public void LoadWorldScene(string world)
    {
        LoadManager.Instance.LoadWorldScene(world);
    }
}