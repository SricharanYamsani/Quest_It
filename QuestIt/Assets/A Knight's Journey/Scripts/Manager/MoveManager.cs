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
        if (m_Choice.AttackStyle == ChoiceStyle.ATTACK)
        {
            int x = UnityEngine.Random.Range(0, 100);
            if (x <= m_Player.attributes.luck.current)
            {
                m_Player.m_PlayerState = PlayerState.BLOCK;
            }
            else
            {
                int damage = Mathf.CeilToInt(((player.attributes.attack.current * 0.25f) + m_Choice.healthChange) - (m_Player.isDefending ? m_Player.attributes.defense.current * 0.65f : m_Player.attributes.defense.current * 0.3f));
                m_Player.attributes.health.current -= Mathf.Clamp(damage, 0, m_Choice.healthChange);
                m_Player.m_PlayerState = PlayerState.NONE;
            }
        }
        else if (m_Choice.AttackStyle == ChoiceStyle.HEAL)
        {
            int heal = m_Choice.healthChange;
            m_Player.attributes.health.current = Mathf.Clamp(m_Player.attributes.health.current + heal, 0, m_Player.attributes.health.maximum);
            Debug.LogError(m_Player.attributes.health.current);
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
