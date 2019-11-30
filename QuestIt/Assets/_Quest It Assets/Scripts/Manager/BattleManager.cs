using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : Singleton<BattleManager>
{
    // VARIABLE SECTION

    [Header ( "-BATTLE STATE-" )]
    public BattleStates battleState = BattleStates.NONE;

    [Header ( "CONSTANTS" )]
    private const int MTime = 15;

    [Header ( "INTEGERS" )]
    public int mTimer = 0;

    [Header ( "BATTLE PLAYER CURRENT LIST" )]
    public List<BattlePlayer> validPlayers = new List<BattlePlayer> ( );

    private void Start ( )
    {
        CalculatePlayersOnField ( );

        GetValidPlayers ( );
    }

    public void CalculatePlayersOnField ( )
    {
        BattlePlayer [ ] temp = FindObjectsOfType<BattlePlayer> ( );

        validPlayers = temp.ToList ( );
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
            // End the Lobby
            break;
        }
    }

    private IEnumerator StartProcess ( )
    {
        mTimer = MTime;

        while ( mTimer > 0 )
        {
            yield return new WaitForSeconds ( 1f );
            mTimer--;
        }
        yield return null;

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
                player.currentChoice.mWork ( );

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
