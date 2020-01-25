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

    /// <summary> Calculate Damages for Every Move</summary>
    /// <param name="player"></param>
    /// <param name="m_Choice"></param>
    /// <param name="m_Player"></param>
    public void CalculateDamage(BattlePlayer player, BattleChoice m_Choice, BattlePlayer m_Player)
    {
        // Deduction Cost from Player For the Move Made
        if(m_Choice.m_Currency == Currency.BRUTE)
        {
            player.attributes.health.current -= m_Choice.m_CurrencyAmount;
        }
        else if(m_Choice.m_Currency == Currency.MANA)
        {
            player.attributes.mana.current -= m_Choice.m_CurrencyAmount;
        }
        else
        {
            Logger.Error("Currency was null");
        }


        // Calculation if player took damage or not.
        if (m_Choice.AttackStyle == ChoiceStyle.ATTACK)
        {
            int x = UnityEngine.Random.Range(0, 100);
            if (x <= m_Player.attributes.luck.current)
            {
                m_Player.m_PlayerState = PlayerState.BLOCK;
            }
            else
            {
                int damage = Mathf.CeilToInt(((player.attributes.attack.current * 0.25f) + m_Choice.healthChange) - (m_Player.IsDefending ? m_Player.attributes.defense.current * 0.65f : m_Player.attributes.defense.current * 0.3f));
                m_Player.attributes.health.current -= Mathf.Clamp(damage, 0, m_Choice.healthChange);
                m_Player.m_PlayerState = PlayerState.NONE;
            }
        }
        else if (m_Choice.AttackStyle == ChoiceStyle.HEAL)
        {
            int heal = m_Choice.healthChange;
            m_Player.attributes.health.current = Mathf.Clamp(m_Player.attributes.health.current + heal, 0, m_Player.attributes.health.maximum);
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
