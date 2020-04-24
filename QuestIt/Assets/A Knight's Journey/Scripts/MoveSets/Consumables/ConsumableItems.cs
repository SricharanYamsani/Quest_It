using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/Consumables - CONSUMABLE")]
public class ConsumableItems : ConsumableChoice
{
    public IWeapon consumableWeapon;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        if (player != null)
        {
            float playTime = 9;
            // deduct -=1;

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
                    IWeapon mObject = Instantiate<IWeapon>(consumableWeapon, targets[i].bottomTransform);

                    mObject.amount = moveAffectDuration;

                    mObject.moveDuration = MoveTurnsType;

                    mObject.Trigger();
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
