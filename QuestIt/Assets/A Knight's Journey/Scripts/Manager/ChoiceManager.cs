using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChoiceManager : Singleton<ChoiceManager>
{
    public event Action<BattlePlayer> OnAddingSelection;

    public event Action<List<BattlePlayer>> OnChoiceSelectionCompleted;

    private List<BattlePlayer> targets = new List<BattlePlayer>();

    protected override void Awake()
    {
        BattleManager.Instance.TurnStart += ResetTargets;
    }

    private void ResetTargets(BattlePlayer player)
    {
        targets.Clear();
    }

    public void AddPlayer(BattlePlayer player, bool raiseEvent = true)
    {
        if (player != null)
        {
            targets.Add(player);

            if (raiseEvent)
            {

                OnAddingSelection?.Invoke(player);
            }

        }
        else
        {
            Logger.Error("Passed player is null");
        }
    }

    public void InvokeRemoveMyPlayer(BattlePlayer player)
    {
        if (targets.Contains(player))
        {
            targets.Remove(player);
        }
    }

    public void OnSelectionCompleted()
    {
        OnChoiceSelectionCompleted?.Invoke(targets);
    }

    public void ExecutePlayerMove(BattlePlayer currentAttacker, BattleChoice move)
    {
        foreach (BattlePlayer target in targets)
        {
            bool isHurt = false;

            if (move.affectedAttribute == AttributeTypes.AGILITY)
            {
                target.CurrentAgility += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move,ref isHurt);
            }
            else if (move.affectedAttribute == AttributeTypes.ATTACK)
            {
                target.CurrentAttack += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move, ref isHurt);
            }
            else if (move.affectedAttribute == AttributeTypes.DEFENSE)
            {
                target.CurrentDefense += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move, ref isHurt);
            }
            else if (move.affectedAttribute == AttributeTypes.HEALTH)
            {
                target.CurrentHealth += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move, ref isHurt);
            }
            else if (move.affectedAttribute == AttributeTypes.LUCK)
            {
                target.CurrentLuck += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move, ref isHurt);
            }
            else if (move.affectedAttribute == AttributeTypes.MANA)
            {
                target.CurrentMana += DamageCalculator.GetChangeUpValue(currentAttacker.playerInfo.myAttributes, target.playerInfo.myAttributes, move, ref isHurt);
            }

            target.SetReaction(isHurt);
        }

        if (move.m_Currency == Currency.BRUTE)
        {
            currentAttacker.CurrentHealth -= move.m_CurrencyAmount;
        }
        else if (move.m_Currency == Currency.MANA)
        {
            currentAttacker.CurrentMana -= move.m_CurrencyAmount;
        }
    }
}
