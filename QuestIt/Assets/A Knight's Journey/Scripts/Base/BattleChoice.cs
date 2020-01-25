using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleChoice : ScriptableObject
{
    public ChoiceStyle AttackStyle = ChoiceStyle.NONE;

    public AttackRange AttackValue = AttackRange.ONEENEMY;

    public MoveDuration MoveTurnsType;

    public AnimationType m_AnimationClip;

    public Currency m_Currency = Currency.NONE;

    public int m_CurrencyAmount = 0;

    public int moveAffectDuration;

    public int healthChange;

    public float endTime = 5f;

    public string moveName;

    public string description = string.Empty;

    public List<BattlePlayer> target = new List<BattlePlayer>();

    public Sprite ICON;

    public virtual void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        BattleManager.Instance.isSelecting = false;
    }
}
