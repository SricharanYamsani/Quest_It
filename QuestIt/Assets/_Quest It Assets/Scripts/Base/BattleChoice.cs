using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BattleChoice : ScriptableObject
{
    public ChoiceStyle AttackStyle = ChoiceStyle.NONE;

    public AttackRange AttackValue = AttackRange.ONEENEMY;

    public MoveDuration MoveTurnsType;

    public int healthChange;

    public AnimationType mAnimationClip;

    public float endTime = 5f;

    public string moveName;

    public int amount;

    public abstract void MoveWork ( );
}
public enum ChoiceStyle
{
    NONE,
    ATTACK,
    DEFEND,
    HEAL
}
