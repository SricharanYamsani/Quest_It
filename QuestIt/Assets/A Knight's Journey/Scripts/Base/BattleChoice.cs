using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleChoice : ScriptableObject
{
    public BattleTasks battleTask = BattleTasks.NONE;

    public AttackRange targetRange = AttackRange.ONEENEMY;

    public PlayerConditions TargetCondition = PlayerConditions.NONE;

    public PercentageType percentageType = PercentageType.NONE;

    public TypesOfChoices choiceType = TypesOfChoices.NONE;

    public AttributeTypes affectedAttribute = AttributeTypes.NONE;

    public Currency m_Currency = Currency.NONE;

    public PlayerCondition playerCondition = PlayerCondition.NORMAL;

    public DamagePercentage damagePercentage = DamagePercentage.NONE;

    public MoveDuration MoveTurnsType;

    public AnimationType m_AnimationClip;

    public Sprite ICON;

    public AudioClip soundClip = null;

    public int m_CurrencyAmount = 0;

    public int moveAffectDuration;

    public int attributeChange;

    public float endTime = 5f;

    public string description = string.Empty;

    public string moveName;

    public virtual void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        BattleManager.Instance.IsSelecting = false;
    }
}
