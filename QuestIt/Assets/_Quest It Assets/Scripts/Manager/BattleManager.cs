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

    [Header ( "-BATTLE STATE-" )]
    public BattleStates battleState = BattleStates.NONE;

    [Header ( "CONSTANTS" )]
    private const int MTime = 15;

    [Header ( "INTEGERS" )]
    public int mTimer = 0;

    public int validPlayersCount = 0;

    private int currentPlayerIndex = -1;

    [Header ( "BATTLE PLAYER CURRENT LIST" )]
    public List<BattlePlayer> validPlayers = new List<BattlePlayer> ( );

    private List<BattlePlayer> roundValidPlayers = new List<BattlePlayer> ( );

    [Header ( "ENEMIES" )]
    public List<BattlePlayer> currentEnemies = new List<BattlePlayer> ( );

    public List<BattlePlayer> currentPlayers = new List<BattlePlayer> ( );

    public List<BattlePlayer> targetPlayers = new List<BattlePlayer> ( );

    [Header ( "CurrentMove - BattlePlayer" )]
    public BattlePlayer currentPlayer = null;

    public BattlePlayer playerPrefabRef;

    public PlayerIcon playerIconRef;

    [Header ( "Transforms" )]
    [Space ( 20 )]
    public Transform [ ] playerSpawn;

    public Transform [ ] enemySpawn;

    public event Action GameInit;

    public event Action TurnOver;

    public event Action<int> TurnStart;

    public event Action RoundOver;

    public event Action GameOver;

    public bool isSelecting = false;

    private void Start ( )
    {
        StartCoroutine ( LoadAllPlayers ( ) );
    }

    IEnumerator LoadAllPlayers()
    {
        yield return null;

        BattlePlayer mPlayer = null;

        for (int i =0 ;i<playerSpawn.Length ;i++ )
        {
            mPlayer = Instantiate ( playerPrefabRef , playerSpawn [ i] );

            //mPlayer.playerIcon = Instantiate<PlayerIcon>(playerIconRef,)
            mPlayer.attributes = PlayerGenerator.Instance.AttributesGenerator ( );

            if ( i == 0 )
            {
                mPlayer.isPlayer = true;
            }
            else
            {
                mPlayer.isPlayer = false;
            }

            mPlayer.isTeam = true;

            mPlayer.tag = "Player";

            validPlayers.Add ( mPlayer );
        }

        for ( int i = 0 ; i < enemySpawn.Length ; i++ )
        {
            mPlayer = Instantiate ( playerPrefabRef , enemySpawn [ i ] );

            mPlayer.isPlayer = false;

            Debug.Log ( i );

            mPlayer.attributes = PlayerGenerator.Instance.AttributesGenerator ( );

            mPlayer.isTeam = false;

            mPlayer.tag = "Enemy";

            mPlayer.transform.rotation = new Quaternion ( 0 , 180 , 0 , 0 );

            validPlayers.Add ( mPlayer );
        }

        CalculatePlayers ( true );

        Sortplayers ( true );

        GameInit?.Invoke ( );

        SwitchPlayState ( BattleStates.CHOICE );

    }

    public void CalculatePlayersOnField ( )
    {
        foreach ( BattlePlayer mEnemies in validPlayers )
        {
            if ( !mEnemies.isTeam )
            {
                currentEnemies.Add ( mEnemies );
            }
            else
            {
                currentPlayers.Add ( mEnemies );
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
    #region Start Battle Rounds
    private IEnumerator StartProcess ( )
    {
        mTimer = MTime;

        //BattleUIManager.Instance.choiceUI.SetActive ( true );

        while ( mTimer > 0 )
        {
            yield return new WaitForSeconds ( 1f );

            mTimer--;
        }
        mTimer = 0;

        yield return null;

        //BattleUIManager.Instance.choiceUI.SetActive ( false );

        SwitchPlayState ( BattleStates.BATTLE );
    }
    #endregion

    public void GetValidPlayers ( )
    {
        List<BattlePlayer> playerList = new List<BattlePlayer> ( );

        int maxSpeed = 0;

        foreach ( BattlePlayer player in validPlayers )
        {
            if ( player.attributes.curHealth <= 0 )
            {
                if ( player.attributes.curAgility >= maxSpeed )
                {
                    maxSpeed = player.attributes.curAgility;
                }

                playerList.Add ( player );

                player.gameObject.SetActive ( false );
            }
        }

        foreach ( BattlePlayer m in playerList )
        {
            validPlayers.Remove ( m );
        }
        if ( validPlayers.Count > 1 )
        {
            for ( int i = 0 ; i < validPlayers.Count - 1 ; i++ )
            {
                for ( int j = 0 ; j < validPlayers.Count - i - 1 ; j++ )
                {
                    if ( validPlayers [ j ].attributes.curAgility < validPlayers [ j + 1 ].attributes.curAgility )
                    {
                        BattlePlayer temp = validPlayers [ j ];

                        validPlayers [ j ] = validPlayers [ j + 1 ];

                        validPlayers [ j + 1 ] = temp;
                    }
                }
            }
        }
        playerList.Clear ( );
    }

    #region Old BattleSystem
    IEnumerator BattleAttackChoice ( )
    {
        // Play Everyone Who Chose Defense
        foreach(BattlePlayer player  in validPlayers)
        {
            if(player.currentChoice.AttackStyle == ChoiceStyle.DEFEND)
            {
                currentPlayer = player;

                //currentPlayer.SetTargets ( );

                currentPlayer.CommitChoice ( );

                yield return new WaitForSeconds ( player.currentChoice.endTime );

                foreach ( BattlePlayer mPlayer in currentPlayer.target )
                {
                    mPlayer.UpdateHealth ( );
                }

                TurnOver?.Invoke ( );
            }
        }
        // Play Everyone Who Chose Attack
        foreach ( BattlePlayer player in validPlayers )
        {
            if ( player.attributes.curHealth > 0 )
            {
                targetPlayers.Clear ( );

                if ( player.currentChoice.AttackStyle == ChoiceStyle.ATTACK )
                {
                    currentPlayer = player;

                    //player.SetTargets ( );

                    targetPlayers.InsertRange ( 0 , player.targetEnemies );

                    player.CommitChoice ( );

                    yield return new WaitForSeconds ( player.currentChoice.endTime );

                    foreach ( BattlePlayer mPlayer in targetPlayers )
                    {
                        mPlayer.UpdateHealth ( );
                    }

                    targetPlayers.Clear ( );

                    TurnOver?.Invoke ( );

                    bool isBack = false;

                    player.mPlayerController.SetBool ( "Walking" , true );

                    player.transform.DOLocalMove ( new Vector3 ( 0 , 0.1f , 0 ) , 0.5f ).OnComplete(()=>{

                        player.mPlayerController.SetBool ( "Walking" , false );

                        player.transform.localPosition = new Vector3 ( 0 , 0.1f , 0 );

                        isBack = true;
                    });

                    while(!isBack)
                    {
                        yield return null;
                    }

                }
            }
            else
            {
                /// Not a Valid entity and needs to be skipped.
            }
        }

        int index = 0;

        foreach ( BattlePlayer player in validPlayers )
        {
            if ( player.attributes.curHealth > 0 )
            {
                index++;
            }
        }

        RoundOver?.Invoke ( );

        if ( index > 1 )
        {
            SwitchPlayState ( BattleStates.CHOICE );
        }
        else
        {
            yield return new WaitForSeconds ( 2.0f );

            SwitchPlayState ( BattleStates.OUTCOME );
        }
    }
    #endregion

    #region New Battle System
    IEnumerator BattleMatch ( )
    {
        currentPlayerIndex = -1;

        for ( int i = 0 ; i < validPlayers.Count ; i++ )
        {
            CalculatePlayers ( );

            isSelecting = true;

            TurnStart?.Invoke ( ++currentPlayerIndex );

            while ( isSelecting )
            {
                yield return null;
            }

            TurnOver?.Invoke ( );
        }

        yield return null;

        RoundOver?.Invoke ( );
    }
    #endregion

    #region Calculate Alive Players
    private void CalculatePlayers ( bool CalculateEnemies = false)
    {
        roundValidPlayers.Clear ( );

        currentEnemies.Clear ( );

        currentPlayers.Clear ( );

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
