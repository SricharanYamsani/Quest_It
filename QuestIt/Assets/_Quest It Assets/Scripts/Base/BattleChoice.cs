using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BattleChoice : ScriptableObject
{
    public ChoiceStyle AttackStyle = ChoiceStyle.NONE;

    public AttackRange AttackValue = AttackRange.ONEENEMY;

    public MoveDuration MoveTurnsType;

    public AnimationType m_AnimationClip;

    public CURRENCY m_Currency = CURRENCY.NONE;

    public int m_CurrencyAmount = 0;

    public int moveAffectDuration;

    public int healthChange;

    public float endTime = 5f;

    public string moveName;

    public string description = string.Empty;

    public List<BattlePlayer> target = new List<BattlePlayer>();

    public Sprite ICON;

    public virtual void MoveWork(List<BattlePlayer> targets)
    {
        BattleManager.Instance.isSelecting = false;
    }
}
public enum ChoiceStyle
{
    NONE,
    ATTACK,
    DEFEND,
    HEAL
}
public enum AttackType
{
    MELEE,
    RANGE,
    MAGIC
}
public enum CURRENCY
{
    NONE,
    MANA,
    BRUTE
}
