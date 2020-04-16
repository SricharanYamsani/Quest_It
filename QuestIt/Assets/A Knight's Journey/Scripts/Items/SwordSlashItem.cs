using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordSlashItem : IWeapon
{
    public override void Trigger(List<BattlePlayer> targets = null)
    {
        base.Trigger(null);

        if (targets != null)
        {
            DOVirtual.DelayedCall(reactionTime, () =>
             {
                 for (int i = 0; i < targets.Count; i++)
                 {
                     targets[i].PlayReaction();
                 }
             });
        }
    }
    public override void RoundSystems ( )
    {
        // Do Something here which is different
        base.RoundSystems ( );
    }
}
