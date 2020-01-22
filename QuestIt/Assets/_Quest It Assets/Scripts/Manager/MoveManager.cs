using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : Singleton<MoveManager>
{
    public event Action<BattlePlayer> TurnAffectOnPlayer;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetMoveDuration(MoveDuration mDuration, int rounds)
    {
        if (mDuration == MoveDuration.TURNS)
        {
            // Affect for turns
        }
        else if (mDuration == MoveDuration.ROUNDS)
        {
            // Affect for Rounds
        }
        else if (mDuration == MoveDuration.NONE)
        {
            // Nothing to happen
        }
    }

    public void CalculateDamage(BattlePlayer player, BattleChoice m_Choice, BattlePlayer m_Player)
    {
        int x = UnityEngine.Random.Range(0, 100);

        if (x <= m_Player.attributes.luck.current)
        {
            m_Player.m_PlayerState = PlayerState.BLOCK;
        }
        else
        {
            if (m_Choice.AttackStyle == ChoiceStyle.ATTACK)
            {
                int damage = Mathf.CeilToInt(((player.attributes.attack.current * 0.25f) + m_Choice.healthChange) - (m_Player.isDefending ? m_Player.attributes.defense.current * 0.65f : m_Player.attributes.defense.current * 0.3f));
                m_Player.attributes.health.current -= damage;
                m_Player.m_PlayerState = PlayerState.NONE;
            }
        }
    }
}

public enum MoveDuration
{
    NONE,
    ROUNDS,
    TURNS
}
