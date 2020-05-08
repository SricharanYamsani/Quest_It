using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightningStrikeItem : IWeapon
{
    public override void Trigger(BattlePlayer targets = null)
    {
        base.Trigger(targets);

        if (targets != null)
        {
            targets.PlayReaction();
        }
    }
}