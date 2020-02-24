using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MoveChoice
{
    public IWeapon lightningEffect;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        foreach(BattlePlayer target in targets)
        {
            IWeapon lightEffect = Instantiate<IWeapon>(lightningEffect, target.bottomTransform);
        }
        base.MoveWork(player, targets);
    }
}
