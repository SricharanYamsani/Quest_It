using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

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

    public event Action GameInit;

    public event Action GameOver;

    private void Start ( )
    {
        CalculatePlayersOnField ( );

        GetValidPlayers ( );

        SwitchPlayState ( BattleStates.CHOICE );

        GameInit?.Invoke ( );
    }

    public void CalculatePlayersOnField ( )
    {
        BattlePlayer [ ] temp = FindObjectsOfType<BattlePlayer> ( );

        validPlayers = temp.ToList ( );

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

            // Check Who Attacks First
            // Play
            //Check If Dead or Not
            //If not, play
            //else
            //end
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
        foreach ( BattlePlayer player in validPlayers )
        {
            if ( player.attributes.curHealth > 0 )
            {
                player.CommitChoice ( );
               
                yield return new WaitForSeconds ( player.currentChoice.endTime );
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

        if ( index > 1 )
        {
            SwitchPlayState ( BattleStates.CHOICE );
        }
        else
        {
            SwitchPlayState ( BattleStates.OUTCOME );
        }
    }

    private void CommunicateChoice (BattlePlayer mPlayer , BattleChoice mChoice)
    {
        switch ( mChoice.AttackStyle )
        {
            case ChoiceStyle.ATTACK:
            mPlayer.target.Health ( -mChoice.healthChange );
            break;

            case ChoiceStyle.DEFEND:
            mPlayer.Health ( mChoice.healthChange );
            break;
        }

        Debug.Log ( ":: CALLED" );

    }
}
public enum BattleStates
{
    NONE,
    CHOICE,
    BATTLE,
    OUTCOME
}
