using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleChoice : ScriptableObject
{
    public ChoiceStyle AttackStyle = ChoiceStyle.NONE;

    public float endTime = 5f;

    public int healthChange;

    public string mAnimationClip;

    public AttackRange AttackValue = AttackRange.ONEENEMY;

    public string moveName;

    public abstract void mWork (BattlePlayer mPlayer);
}
public enum ChoiceStyle
{
    NONE,
    ATTACK,
    DEFEND
}
