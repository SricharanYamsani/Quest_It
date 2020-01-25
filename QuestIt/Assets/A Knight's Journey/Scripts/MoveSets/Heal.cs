using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - HEAL")]
public class Heal : BattleChoice
{
    public IWeapon healWeapon;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        if (player != null)
        {
            IWeapon mObject = Instantiate<IWeapon>(healWeapon, player.rightHandSpawnInside);

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger();

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                player.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(endTime, () => {; }));

            mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });

            foreach (BattlePlayer m_Player in targets)
            {
                MoveManager.Instance.CalculateDamage(player, this, m_Player);
            }
        }
    }
}
