using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleItems : ScriptableObject
{
    public BattleTasks AttackStyle = BattleTasks.NONE;

    public AttackRange AttackValue = AttackRange.ONEENEMY;

    public AnimationType m_AnimationClip;

    public int healthChange;

    public float endTime = 5f;

    public string itemName;

    public string description = string.Empty;

    public Sprite ICON;

    /// <summary> Update Battle Manager for Selection </summary>
    /// <param name="targets"> Affected Targets </param>
    public virtual void MoveWork(List<BattlePlayer> targets)
    {
        BattleManager.Instance.IsSelecting = false;
    }
}
