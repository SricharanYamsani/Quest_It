﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using DG.Tweening;
using RPG.CameraControl;

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

    #region Camera Related

    public CameraController cameraController;

    #endregion

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
        Sortplayers();
    }

    public void InitializeBattle(List<PlayerInfo> allPlayers)
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
            List<PlayerInfo> playerQ = new List<PlayerInfo>();

            for (int i = 0; i < 6; i++)
            {
                PlayerInfo player = new PlayerInfo();

                player.myAttributes = PlayerGenerator.AttributesGenerator();

                player.character = (BattleCharacters)UnityEngine.Random.Range(1, 6);
                //BattleCharacters.OCCULTIST;

                player.chosenMoves.Add(Moves.SWIPE_SLASH);
                player.chosenMoves.Add(Moves.PIERCE_ATTACK_1);
                player.chosenMoves.Add(Moves.MAGIC_HEAL_SMALL_1);

                player.IsTeamRed = (i < 3);

                player.IsPlayer = (i == 0 ? true : false);

                ConsumablesInfo cInfo = new ConsumablesInfo();

                cInfo.consumable = Consumables.RESURRECT_SMALL_1;

                cInfo.amount = 1;

                player.consumables.Add(cInfo);

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
            if (IsTeamAlive(CurrentBlueTeam) && IsTeamAlive(CurrentRedTeam))
            {
                currentPlayer = validPlayers[i];

                IsSelecting = true;

                currentPlayer.PerformMoveFocus(true);

                TurnStart?.Invoke(currentPlayer);

                while (IsSelecting)
                {
                    yield return null;
                }
                currentPlayer.PerformMoveFocus(false);

                TurnOver?.Invoke();
            }
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

    private void SetupPlayersOnField(List<PlayerInfo> qualities)
    {
        foreach (PlayerInfo quality in qualities)
        {
            Transform myTransform = GetSpawn(quality.IsTeamRed, quality.IsPlayer);

            if (myTransform != null)
            {
                BattlePlayer player = Instantiate<BattlePlayer>(ResourceManager.Instance.allModels[quality.character], myTransform);

                player.playerInfo = quality;

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

    public List<BattlePlayer> GetTeamPlayers(BattlePlayer _currentPlayer = null)
    {
        if (_currentPlayer == null)
        {
            _currentPlayer = currentPlayer;
        }

        List<BattlePlayer> teamPlayers = new List<BattlePlayer>();

        foreach (BattlePlayer player in validPlayers)
        {
            if (_currentPlayer.IsTeamRed == player.IsTeamRed && _currentPlayer != player)
            {
                teamPlayers.Add(player);
            }
        }
        return teamPlayers;
    }

    public List<Transform> GetAllPlayersTransform()
    {
        List<Transform> playerTransforms = new List<Transform>();

        foreach (BattlePlayer player in validPlayers)
        {
            playerTransforms.Add(player.transform);
        }

        return playerTransforms;
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
            if (isPlayer)
            {
                if (spawns[i].isPlayerSpot)
                {
                    return spawns[i].spawn;
                }
            }
            else
            {
                if (!spawns[i].IsOccupied && spawns[i].isTeamRed == isTeamRed && !spawns[i].isPlayerSpot)
                {
                    spawns[i].IsOccupied = true;

                    return spawns[i].spawn;
                }
            }
        }

        return null;
    }

    public List<BattlePlayer> GetTargetPlayers(BattleChoice choice, ref bool canSelect)
    {
        List<BattlePlayer> targets = new List<BattlePlayer>();

        switch (choice.targetRange)
        {
            case AttackRange.ONEENEMY:
            case AttackRange.ONETEAM:
                canSelect = true;
                break;
            case AttackRange.ALLENEMY:
            case AttackRange.ALLTEAM:
            case AttackRange.EVERYONE:
                canSelect = false;
                break;
            default:throw new Exception("Invalid Target Range : Wrong Info in selected move");
        }

        if (choice.playerCondition == PlayerConditions.ALIVE)
        {
            if (choice.targetRange == AttackRange.ALLENEMY || choice.targetRange == AttackRange.ONEENEMY)
            {
                foreach (BattlePlayer target in validPlayers)
                {
                    if ((currentPlayer.IsTeamRed != target.IsTeamRed) && (target.IsAlive))
                    {
                        targets.Add(target);
                    }
                }
            }
            else if (choice.targetRange == AttackRange.ALLTEAM || choice.targetRange == AttackRange.ONETEAM)
            {
                foreach (BattlePlayer target in validPlayers)
                {
                    if ((currentPlayer.IsTeamRed == target.IsTeamRed) && (target.IsAlive))
                    {
                        targets.Add(target);
                    }
                }
            }
        }
        else if (choice.playerCondition == PlayerConditions.DEAD)
        {
            if (choice.targetRange == AttackRange.ALLENEMY || choice.targetRange == AttackRange.ONEENEMY)
            {
                foreach (BattlePlayer target in validPlayers)
                {
                    if ((currentPlayer.IsTeamRed != target.IsTeamRed) && (!target.IsAlive))
                    {
                        targets.Add(target);
                    }
                }
            }
            else if (choice.targetRange == AttackRange.ALLTEAM || choice.targetRange == AttackRange.ONETEAM)
            {
                foreach (BattlePlayer target in validPlayers)
                {
                    if ((currentPlayer.IsTeamRed == target.IsTeamRed) && (!target.IsAlive))
                    {
                        targets.Add(target);
                    }
                }
            }
        }
        return targets;
    }
}
public enum BattleStates
{
    NONE,
    INIT,
    BATTLE,
    OUTCOME
}
