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

    [Header ( "BATTLE PLAYER CURRENT LIST" )]
    public List<BattlePlayer> validPlayers = new List<BattlePlayer> ( );

    [Header ( "ENEMIES" )]
    public List<BattlePlayer> currentEnemies = new List<BattlePlayer> ( );

    public List<BattlePlayer> currentPlayers = new List<BattlePlayer> ( );

    public List<BattlePlayer> targetPlayers = new List<BattlePlayer> ( );

    [Header ( "CurrentMove - BattlePlayer" )]
    public BattlePlayer currentPlayer = null;

    public BattlePlayer playerPrefabRef;

    [Header ( "Transforms" )]
    [Space ( 20 )]
    public Transform [ ] playerSpawn;

    public Transform [ ] enemySpawn;

    public event Action GameInit;

    public event Action TurnOver;

    public event Action RoundOver;

    public event Action GameOver;

    private void Start ( )
    {
        StartCoroutine ( LoadAllPlayers ( ) );
    }

    IEnumerator LoadAllPlayers()
    {
        yield return null;

        BattlePlayer mPlayer = Instantiate ( playerPrefabRef , playerSpawn [ 0 ] );

        mPlayer.isPlayer = true;

        mPlayer.tag = "Player";

        validPlayers.Add ( mPlayer );

        mPlayer = Instantiate ( playerPrefabRef , enemySpawn [ 0 ] );

        mPlayer.isPlayer = false;

        mPlayer.tag = "Enemy";

        mPlayer.transform.rotation = new Quaternion ( 0 , 180 , 0 , 0 );

        validPlayers.Add ( mPlayer );

        CalculatePlayersOnField ( );

        GetValidPlayers ( );

        SwitchPlayState ( BattleStates.CHOICE );

        GameInit?.Invoke ( );

    }

    public void CalculatePlayersOnField ( )
    {
        foreach(BattlePlayer mEnemies in validPlayers)
        {
            if(!mEnemies.isPlayer)
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

            StartCoroutine ( BattleAttackChoice ( ) );

            break;

            case BattleStates.OUTCOME:

            // Show the Outcome
            GameOver?.Invoke ( );
            // End the Lobby
            break;
        }
    }

    private IEnumerator StartProcess ( )
    {
        mTimer = MTime;

        BattleUIManager.Instance.choiceUI.SetActive ( true );

        while ( mTimer > 0 )
        {
            yield return new WaitForSeconds ( 1f );

            mTimer--;
        }
        yield return null;

        BattleUIManager.Instance.choiceUI.SetActive ( false );

        SwitchPlayState ( BattleStates.BATTLE );
    }

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

    IEnumerator BattleAttackChoice ( )
    {
        // Play Everyone Who Chose Defense
        foreach(BattlePlayer player  in validPlayers)
        {
            if(player.currentChoice.AttackStyle == ChoiceStyle.DEFEND)
            {
                currentPlayer = player;

                currentPlayer.SetTargets ( );

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

                    player.SetTargets ( );

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
            SwitchPlayState ( BattleStates.OUTCOME );
        }
    }

    public void TriggerTargetPlayer()
    {
        foreach(BattlePlayer player in targetPlayers)
        {
            player.ShowReaction ( );
        }
    }
}
public enum BattleStates
{
    NONE,
    CHOICE,
    BATTLE,
    OUTCOME
}
