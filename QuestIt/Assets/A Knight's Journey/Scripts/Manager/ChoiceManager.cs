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
        if(targets.Contains(player))
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
        if (move.AttackStyle == BattleTasks.ATTACK)
        {
            foreach (BattlePlayer target in targets)
            {
                if (DamageCalculator.IsDodge(target.CurrentLuck))
                {
                    // Set Dodge Animation
                    target.SetReaction(PlayerState.BLOCK);
                }
                else
                {
                    target.SetReaction(PlayerState.HURT);

                    target.CurrentHealth -= DamageCalculator.GetDamage(currentAttacker.playerQualities.myAttributes, target.playerQualities.myAttributes, move.healthChange);

                    target.CurrentHealth = Mathf.Clamp(target.CurrentHealth, 0, target.MaxHealth);
                }
            }
        }
        else if(move.AttackStyle == BattleTasks.HEAL)
        {
            foreach(BattlePlayer target in targets)
            {
                target.CurrentHealth += move.healthChange;

                target.CurrentHealth = Mathf.Clamp(target.CurrentHealth, 0, target.MaxHealth);
            }
        }

        if(move.m_Currency == Currency.BRUTE)
        {
            currentAttacker.CurrentHealth -= move.m_CurrencyAmount;
        }
        else if(move.m_Currency == Currency.MANA)
        {
            currentAttacker.CurrentMana -= move.m_CurrencyAmount;
        }
    }
}
