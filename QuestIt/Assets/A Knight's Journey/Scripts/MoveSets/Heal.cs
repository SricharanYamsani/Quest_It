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
            float playTime = endTime;

            Sequence mySequence = DOTween.Sequence();

            if (targets[0].CurrentHealth <= 0)
            {
                playTime += 8;
            }

            Quaternion tempRotation = player.transform.rotation;

            if(!targets.Contains(player))
            {
                player.transform.DOLookAt(targets[0].transform.position, 0.4f);
            }

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                player.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(1f,()=> {

                IWeapon mObject = Instantiate<IWeapon>(healWeapon, targets[0].bottomTransform);

                mObject.amount = moveAffectDuration;

                mObject.moveDuration = MoveTurnsType;

                mObject.Trigger();

            })).Append(DOVirtual.DelayedCall(playTime, () => {
                player.transform.DORotateQuaternion(tempRotation, 0.3f);

            }));

            mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });

            ChoiceManager.Instance.ExecutePlayerMove(player, this);
        }
    }
}
