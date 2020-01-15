using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : Singleton<MoveManager>
{
    public event Action<BattlePlayer> TurnAffectOnPlayer;

    protected override void Awake ( )
    {
        base.Awake ( );
    }

    public void SetMoveDuration (MoveDuration mDuration , int rounds)
    {
        if ( mDuration == MoveDuration.TURNS )
        {
            // Affect for turns
        }
        else if ( mDuration == MoveDuration.ROUNDS )
        {
            // Affect for Rounds
        }
        else if ( mDuration == MoveDuration.NONE )
        {
            // Nothing to happen
        }
    }

    public void CalculateDamage (BattleChoice m_Choice , BattlePlayer m_Player)
    {
        int x = UnityEngine.Random.Range ( 0 , 100 );

        if ( x <= m_Player.attributes.curLuck )
        {
            m_Player.m_PlayerState = PlayerState.BLOCK;
        }
        else
        {
            m_Player.attributes.curHealth -= m_Choice.healthChange;

            m_Player.m_PlayerState = PlayerState.NONE;
        }
    }
}

public enum MoveDuration
{
    NONE,
    ROUNDS,
    TURNS
}
