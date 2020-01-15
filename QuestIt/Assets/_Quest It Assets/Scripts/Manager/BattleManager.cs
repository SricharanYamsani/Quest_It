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

    public List<BattlePlayer> targetPlayers = new List<BattlePlayer>();

    [Header("CurrentMove - BattlePlayer")]
    public BattlePlayer currentPlayer = null;

    public BattlePlayer playerPrefabRef;

    public PlayerIcon playerIconRef;

    [Header("Transforms")]
    [Space(20)]
    public Transform[] playerSpawn;

    public Transform[] enemySpawn;

    public event Action GameInit;

    public event Action TurnOver;

    public event Action<BattlePlayer> TurnStart; // send the player 

    public event Action RoundOver;

    public event Action GameOver;

    public bool isSelecting { get; set; } = false;

    public static int uniqueID = 0;

    protected override void Awake()
    {
        base.Awake();

        RoundOver += RoundOverFunc;

        TurnStart += TurnStartFunc;
    }

    private void RoundOverFunc() // Round Over Method  - Call It EveryTime and Check for Game Over
    {
        Debug.LogWarning("Round was Over");

        int hasHealth = 0;

        foreach (BattlePlayer player in roundValidPlayers)
        {
            if (player.attributes.curHealth > 0)
            {
                hasHealth++;
            }
        }

        if (hasHealth > 1)
        {
            SwitchPlayState(BattleStates.BATTLE);
        }
        else
        {
            Debug.LogError("Game Over");
        }
    }

    private void TurnStartFunc(BattlePlayer player) // Turn Over Method
    {
        Debug.LogWarning(player);
    }

    private void Start ( )
    {
        StartCoroutine ( LoadAllPlayers ( ) ); // Remove It from here need a better process or make the system completely on Ienumerator
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
                t_Player.isPlayer = true;
            }
            else
            {
                t_Player.isPlayer = false;
            }

            t_Player.isTeamRed = true;

            t_Player.tag = "Player";

            t_Player.OriginalSpawn = playerSpawn[i];

            validPlayers.Add ( t_Player );
        }

        for ( int i = 0 ; i < enemySpawn.Length ; i++ )
        {
            t_Player = Instantiate ( playerPrefabRef , enemySpawn [ i ] );

            t_Player.isPlayer = false;

            Debug.Log ( i );

            t_Player.attributes = PlayerGenerator.Instance.AttributesGenerator ( );

            t_Player.isTeamRed = false;

            t_Player.OriginalSpawn = enemySpawn[i];

            t_Player.tag = "Enemy";

            t_Player.transform.rotation = new Quaternion ( 0 , 180 , 0 , 0 );

            validPlayers.Add ( t_Player );
        }

        CalculatePlayers ( true );

        Sortplayers ( true );

        GameInit?.Invoke ( );

        SwitchPlayState ( BattleStates.CHOICE );

    }
    #endregion
    public void CalculatePlayersOnField ( ) // Calculate if it is Team Red or Team Blue
    {
        foreach ( BattlePlayer t_Enemies in validPlayers )
        {
            if ( !t_Enemies.isTeamRed )
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

            case BattleStates.CHOICE:

            StartCoroutine ( StartProcess ( ) );

            break;

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
    // In Process of Removing it
    #region Start Battle Rounds 
    private IEnumerator StartProcess ( )
    {
        m_Timer = MTime;

        //BattleUIManager.Instance.choiceUI.SetActive ( true );

        while ( m_Timer > 0 )
        {
            yield return new WaitForSeconds ( 1f );

            m_Timer--;
        }
        m_Timer = 0;

        yield return null;

        //BattleUIManager.Instance.choiceUI.SetActive ( false );

        SwitchPlayState ( BattleStates.BATTLE );
    }
    #endregion

    #region New Battle System
    IEnumerator BattleMatch()
    {
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
            if ( validPlayers [ i ].attributes.GetCurrentHealth ( ) > 0 )
            {
                validPlayers [ i ].TakePartInBattle ( true );

                roundValidPlayers.Add ( validPlayers [ i ] );
            }
            else
            {
                validPlayers [ i ].TakePartInBattle ( false );

                validPlayers [ i ].turnIndex = -99;
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
                    if ( roundValidPlayers [ j ].currentAgility > roundValidPlayers [ j + 1 ].currentAgility )
                    {
                        BattlePlayer temp = roundValidPlayers [ j ];

                        roundValidPlayers [ j ] = roundValidPlayers [ j + 1 ];

                        roundValidPlayers [ j ] = temp;
                    }
                }
                else
                {
                    if ( roundValidPlayers [ j ].currentAgility < roundValidPlayers [ j + 1 ].currentAgility )
                    {
                        BattlePlayer temp = roundValidPlayers [ j ];

                        roundValidPlayers [ j ] = roundValidPlayers [ j + 1 ];

                        roundValidPlayers [ j ] = temp;
                    }
                }
            }
        }

        for ( int i = 0 ; i < roundValidPlayers.Count ; i++ )
        {
            roundValidPlayers [ i ].turnIndex = i;
        }
    }
    #endregion
}
public enum BattleStates
{
    NONE,
    CHOICE,
    BATTLE,
    OUTCOME
}
