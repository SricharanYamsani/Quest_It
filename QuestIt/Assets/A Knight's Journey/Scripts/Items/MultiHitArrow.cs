using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MultiHitArrow : IWeapon
{
    public override void Trigger(BattlePlayer targets = null)
    {
        if (targets != null)
        {
            DOVirtual.DelayedCall(3.15f, () =>
            {
                transform.DOMove(targets.torsoTransform.position, 0.5f);
            });

            DOVirtual.DelayedCall(3.66f, () =>
            {
                targets.PlayReaction();

                DOVirtual.DelayedCall(0.1f, () => { RoundSystems(); });

            });
        }
    }
    

    public override void RoundSystems()
    {
        Destroy(gameObject);
    }
}