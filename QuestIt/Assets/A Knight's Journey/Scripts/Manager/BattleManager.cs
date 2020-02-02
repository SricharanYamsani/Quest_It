using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class BattleManager : Singleton<BattleManager>
{
    // VARIABLE SECTION

    public BattleStates battleState = BattleStates.NONE;

    public int validPlayersCount = 0;

    private List<BattlePlayer> validPlayers = new List<BattlePlayer>();

    private List<BattlePlayer> CurrentRedTeam { get; set; } = new List<BattlePlayer>();

    private List<BattlePlayer> CurrentBlueTeam { get; set; } = new List<BattlePlayer>();

    public BattlePlayer currentPlayer = null;

    public BattlePlayer playerPrefabRef;

    public List<SpawnPoint> spawns = new List<SpawnPoint>();

    // Events 
    public event Action GameInit;

    public event Action TurnOver;

    public event Action<BattlePlayer> TurnStart;

    public event Action RoundStart;

    public event Action RoundOver;

    public event Action GameOver;

    // Bool for selection
    public bool IsSelecting { get; set; } = false;

    public bool IsLoaded { get; private set; } = false;

    public bool LoadFromScene;

    public static int uniqueID = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void RoundOverFunc() // Round Over Method  - Call It EveryTime and Check for Game Over
    {
        if (IsTeamAlive(CurrentRedTeam) && IsTeamAlive(CurrentBlueTeam))
        {
            SwitchPlayState(BattleStates.BATTLE);
        }
        else
        {
            GameOver?.Invoke();
        }
    }

    private bool IsTeamAlive(List<BattlePlayer> players)
    {
        bool isAlive = false;

        for (int i = players.Count - 1; i >= 0; --i)
        {
            if (players[i].IsAlive)
            {
                isAlive = true;
                break;
            }
        }
        return isAlive;
    }

    private void TurnStartFunc(BattlePlayer player) // Turn Over Method
    {

    }

    private void RoundStartFunc()
    {

    }

    public void InitializeBattle(List<PlayerQualities> allPlayers)
    {
        ResetSystem();

        SetupPlayersOnField(allPlayers);
    }



    private void Start() // this will have to go.
    {
        Application.targetFrameRate = 60;

        QualitySettings.vSyncCount = 0;

        if (InformationHandler.Instance.lobbyPlayers.Count <= 0)
        {
            GeneratePlayers();
        }

        InitializeBattle(InformationHandler.Instance.lobbyPlayers);
    }

    private void GeneratePlayers()
    {
        if (LoadFromScene)
        {
            List<PlayerQualities> playerQ = new List<PlayerQualities>();

            for (int i = 0; i < 6; i++)
            {
                PlayerQualities player = new PlayerQualities();

                player.myAttributes = PlayerGenerator.AttributesGenerator();

                player.character = BattleCharacters.BESTOFWORLDS;

                for (int j = 1; j <= 4; j++)
                {
                    player.chosenMoves.Add((MOVES)j);
                }

                player.IsTeamRed = (i < 3);

                player.IsPlayer = (i == 0 ? true : false);

                playerQ.Add(player);
            }

            InformationHandler.Instance.SetLobby(playerQ);
        }
    }

    private void Setup()
    {
        RoundOver += RoundOverFunc;

        TurnStart += TurnStartFunc;

        RoundStart += RoundStartFunc;

        StartCoroutine(LoadAllPlayers()); // Remove It from here need a better process or make the system completely on Ienumerator
    }


    #region Loading all Players On The Field (MAX - 3 OF EACH TEAM)
    IEnumerator LoadAllPlayers()
    {
        yield return null;

        CalculateTeamPlayers();

        Sortplayers();

        GameInit?.Invoke();

        SwitchPlayState(BattleStates.BATTLE);
    }
    #endregion
    public void CalculateTeamPlayers() // Calculate if it is Team Red or Team Blue
    {
        foreach (BattlePlayer player in validPlayers)
        {
            player.TakePartInBattle(true);

            player.SetPlayer();

            if (!player.IsTeamRed)
            {
                CurrentRedTeam.Add(player);
            }
            else
            {
                CurrentBlueTeam.Add(player);
            }
        }
    }
    public void SwitchPlayState(BattleStates mState)
    {
        battleState = mState;

        PlayState();
    }

    public void PlayState()
    {
        switch (battleState)
        {
            case BattleStates.NONE: break;

            case BattleStates.BATTLE:

                StartCoroutine(BattleMatch());

                break;

            case BattleStates.OUTCOME:

                // Show the Outcome
                GameOver?.Invoke();
                // End the Lobby
                break;
        }
    }

    #region New Battle System
    IEnumerator BattleMatch()
    {
        RoundStart?.Invoke();

        for (int i = 0; i < validPlayers.Count; i++)
        {
            currentPlayer = validPlayers[i];

            IsSelecting = true;

            TurnStart?.Invoke(currentPlayer);

            while (IsSelecting)
            {
                yield return null;
            }

            TurnOver?.Invoke();
        }

        yield return null;

        RoundOver?.Invoke();
    }
    #endregion

    #region Player Sorting
    private void Sortplayers() //  Sorting Players To get 
    {
        validPlayers.Sort((player1, player2) =>
        {
            return (player2.CurrentAgility.CompareTo(player1.CurrentAgility));
        });
    }
    #endregion

    private void SetupPlayersOnField(List<PlayerQualities> qualities)
    {
        foreach (PlayerQualities quality in qualities)
        {
            Transform myTransform = GetSpawn(quality.IsTeamRed, quality.IsPlayer);

            if (myTransform != null)
            {
                BattlePlayer player = Instantiate<BattlePlayer>(ResourceManager.Instance.allModels[quality.character], myTransform);

                player.playerQualities = quality;

                validPlayers.Add(player);
            }
            else
            {
                Debug.Log(quality.IsPlayer + " : " + "Cannot find appropriate spawn. Passing players > 6?");
            }
        }
        Setup();
    }

    private void ResetSystem()
    {
        validPlayers.Clear();

        CurrentRedTeam.Clear();

        CurrentBlueTeam.Clear();

        foreach (SpawnPoint spawn in spawns)
        {
            spawn.IsOccupied = false;
        }
    }

    public List<BattlePlayer> GetTeamRedPlayers()
    {
        return CurrentRedTeam;
    }

    public List<BattlePlayer> GetTeamBluePlayers()
    {
        return CurrentBlueTeam;
    }

    public List<BattlePlayer> GetAllPlayers()
    {
        return validPlayers;
    }

    /// <summary>
    /// Get available spawns from the spawn list.
    /// </summary>
    /// <param name="isTeamRed"> Look for spawns in team "Red" or "Blue".</param>
    /// <param name="isPlayer"> Look for player spawn - can be only one.</param>
    /// <returns></returns>
    private Transform GetSpawn(bool isTeamRed, bool isPlayer = false)
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            if (isTeamRed)
            {
                if (isPlayer)
                {
                    if (spawns[i].isPlayerSpot)
                    {
                        return spawns[i].spawn;
                    }
                }
                else
                {
                    if (!spawns[i].IsOccupied && spawns[i].isTeamRed && !spawns[i].isPlayerSpot)
                    {
                        spawns[i].IsOccupied = true;

                        return spawns[i].spawn;
                    }
                }
            }
            else
            {
                if (isPlayer)
                {
                    if (spawns[i].isPlayerSpot)
                    {
                        return spawns[i].spawn;
                    }
                }
                else
                {
                    if (!spawns[i].IsOccupied && !spawns[i].isTeamRed && !spawns[i].isPlayerSpot)
                    {
                        spawns[i].IsOccupied = true;

                        return spawns[i].spawn;
                    }
                }
            }
        }

        return null;
    }
}
public enum BattleStates
{
    NONE,
    INIT,
    BATTLE,
    OUTCOME
}
