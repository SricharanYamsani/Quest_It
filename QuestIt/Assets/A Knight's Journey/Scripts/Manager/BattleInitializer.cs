using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : Singleton<BattleInitializer>
{
    public List<BattlePlayer> lobbyPlayers = new List<BattlePlayer>();

    /// <summary>Adds a Battle Player to the Lobby. </summary>
    /// <param name="player"> Battle Player</param>

    protected override void Awake()
    {
        base.Awake();

        isDontDestroyOnLoad = true;
    }
    public void AddaBattlePlayer(BattlePlayer player)
    {
        if (lobbyPlayers == null)
        {
            lobbyPlayers = new List<BattlePlayer>();
        }

        lobbyPlayers.Add(player);
    }

    public bool InitializeLobby()
    { 
        if (lobbyPlayers.Count > 1)
        {
            BattleManager.Instance.InitializeBattle(lobbyPlayers);

            lobbyPlayers = null;

            return true;
        }

        return false;
    }
}
