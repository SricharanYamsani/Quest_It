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

    [Header("-BATTLE STATE-")]
    public BattleStates battleState = BattleStates.NONE;

    [Header("CONSTANTS")]
    private const int MTime = 0;

    [Header("INTEGERS")]
    public int m_Timer = 0;

    public int validPlayersCount = 0;

    private int currentPlayerIndex = -1;

    [Header("BATTLE PLAYER CURRENT LIST")]
    public List<BattlePlayer> validPlayers = new List<BattlePlayer>();

    private List<BattlePlayer> roundValidPlayers = new List<BattlePlayer>();

    [Header("ENEMIES")]
    public List<BattlePlayer> currentRed = new List<BattlePlayer>();

    public List<BattlePlayer> currentBlue = new List<BattlePlayer>();

    [Header("CurrentMove - BattlePlayer")]
    public BattlePlayer currentPlayer = null;

    public BattlePlayer playerPrefabRef;

    [Header("Transforms")]
    [Space(20)]
    public Transform[] playerSpawn;

    public Transform[] enemySpawn;


    // Events 
    public event Action GameInit;

    public event Action TurnOver;

    public event Action<BattlePlayer> TurnStart;

    public event Action RoundStart;

    public event Action RoundOver;

    public event Action GameOver;

    // Bool for selection
    public bool isSelecting { get; set; } = false;

    public static int uniqueID = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void RoundOverFunc() // Round Over Method  - Call It EveryTime and Check for Game Over
    {

        if (IsTeamAlive(currentRed) && IsTeamAlive(currentBlue))
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
            if (players[i].attributes.health.current > 0)
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

    private void Start() // this will have to go.
    {
        StartBattle();
    }
    public void StartBattle()
    {
        Setup();
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

        BattlePlayer t_Player = null;

        for (int i =0 ;i<playerSpawn.Length ;i++ )
        {
            t_Player = Instantiate ( playerPrefabRef , playerSpawn [ i] );

            //mPlayer.playerIcon = Instantiate<PlayerIcon>(playerIconRef,)
            t_Player.attributes = PlayerGenerator.Instance.AttributesGenerator ( );

            if ( i == 0 )
            {
                t_Player.SetPlayer(true, true);
            }
            else
            {
                t_Player.SetPlayer(false, true);
            }

            t_Player.tag = "Player";

            t_Player.OriginalSpawn = playerSpawn[i];

            validPlayers.Add ( t_Player );
        }

        for ( int i = 0 ; i < enemySpawn.Length ; i++ )
        {
            t_Player = Instantiate ( playerPrefabRef , enemySpawn [ i ] );

            t_Player.SetPlayer(false, false);

            t_Player.attributes = PlayerGenerator.Instance.AttributesGenerator ( );

            t_Player.OriginalSpawn = enemySpawn[i];

            t_Player.tag = "Enemy";

            t_Player.transform.rotation = new Quaternion ( 0 , 180 , 0 , 0 );

            validPlayers.Add ( t_Player );
        }

        CalculatePlayers ( true );

        Sortplayers ( true );

        GameInit?.Invoke ( );

        SwitchPlayState ( BattleStates.BATTLE );

    }
    #endregion
    public void CalculatePlayersOnField ( ) // Calculate if it is Team Red or Team Blue
    {
        foreach ( BattlePlayer t_Enemies in validPlayers )
        {
            if ( !t_Enemies.IsTeamRed )
            {
                currentRed.Add ( t_Enemies );
            }
            else
            {
                currentBlue.Add ( t_Enemies );
            }
        }
    }
    public void SwitchPlayState (BattleStates mState)
    {
        battleState = mState;

        PlayState ( );
    }

    public void PlayState ( )
    {
        switch ( battleState )
        {
            case BattleStates.NONE: break;

            case BattleStates.BATTLE:

            StartCoroutine ( BattleMatch ( ) );

            break;

            case BattleStates.OUTCOME:

            // Show the Outcome
            GameOver?.Invoke ( );
            // End the Lobby
            break;
        }
    }

    #region New Battle System
    IEnumerator BattleMatch()
    {
        RoundStart?.Invoke();

        currentPlayerIndex = -1;

        for (int i = 0; i < roundValidPlayers.Count; i++)
        {
            currentPlayer = roundValidPlayers[i];

            isSelecting = true;

            TurnStart?.Invoke(currentPlayer);

            while (isSelecting)
            {
                yield return null;
            }

            TurnOver?.Invoke();

            yield return new WaitForSeconds(2);
        }

        yield return null;

        RoundOver?.Invoke();
    }
    #endregion

    #region Calculate Alive Players
    private void CalculatePlayers ( bool CalculateEnemies = false)
    {
        roundValidPlayers.Clear ( );

        currentRed.Clear ( );

        currentBlue.Clear ( );

        for ( int i = 0 ; i < validPlayers.Count ; i++ )
        {
            if ( validPlayers [ i ].attributes.health.current  > 0 )
            {
                validPlayers [ i ].TakePartInBattle ( true );

                roundValidPlayers.Add ( validPlayers [ i ] );
            }
            else
            {
                validPlayers [ i ].TakePartInBattle ( false );
            }
        }

        if(CalculateEnemies)
        {
            CalculatePlayersOnField ( );
        }
    }
    #endregion

    #region Player Sorting
    private void Sortplayers (bool isDescending = true) //  Sorting Players To get 
    {
        for ( int i = 0 ; i < roundValidPlayers.Count - 1 ; i++ )
        {
            for ( int j = 0 ; j < roundValidPlayers.Count - 1 - i ; j++ )
            {
                if ( isDescending )
                {
                    if ( roundValidPlayers [ j ].CurrentAgility > roundValidPlayers [ j + 1 ].CurrentAgility )
                    {
                        BattlePlayer temp = roundValidPlayers [ j ];

                        roundValidPlayers [ j ] = roundValidPlayers [ j + 1 ];

                        roundValidPlayers [ j ] = temp;
                    }
                }
                else
                {
                    if ( roundValidPlayers [ j ].CurrentAgility < roundValidPlayers [ j + 1 ].CurrentAgility )
                    {
                        BattlePlayer temp = roundValidPlayers [ j ];

                        roundValidPlayers [ j ] = roundValidPlayers [ j + 1 ];

                        roundValidPlayers [ j ] = temp;
                    }
                }
            }
        }
    }
    #endregion
}
public enum BattleStates
{
    NONE,
    INIT,
    BATTLE,
    OUTCOME
}
