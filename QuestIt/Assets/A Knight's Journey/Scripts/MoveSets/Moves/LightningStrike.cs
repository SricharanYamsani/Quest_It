using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - LIGHTNING STRIKE")]
public class LightningStrike : MoveChoice
{
    public IWeapon lightningEffect;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        if (player != null)
        {
            float playTime = endTime;

            Sequence mySequence = DOTween.Sequence();

            Quaternion tempRotation = player.transform.rotation;

            if (!targets.Contains(player))
            {
                player.transform.DOLookAt(targets[0].transform.position, 0.4f);
            }

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                player.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(1f, () =>
            {

                for (int i = 0; i < targets.Count; i++)
                {
                    IWeapon mObject = Instantiate<IWeapon>(lightningEffect, targets[i].bottomTransform);

                    mObject.amount = moveAffectDuration;

                    mObject.moveDuration = MoveTurnsType;

                    List<BattlePlayer> rTargets = new List<BattlePlayer>();

                    rTargets.Add(targets[i]);

                    mObject.Trigger(rTargets);
                }

            })).Append(DOVirtual.DelayedCall(playTime, () =>
            {
                player.transform.DORotateQuaternion(tempRotation, 0.3f);

            }));

            mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });
        }
    }
}
