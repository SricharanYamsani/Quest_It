using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - HEAL")]
public class Heal : MoveChoice
{
    public IWeapon healWeapon;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> targets)
    {
        if (player != null)
        {
            float playTime = endTime;

            Sequence mySequence = DOTween.Sequence();

            foreach (BattlePlayer target in targets)
            {
                if (target.CurrentHealth <= 0)
                {
                    Debug.Log(target.CurrentHealth);

                    playTime += 8;

                    break;
                }
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

                for (int i = 0; i < targets.Count; i++)
                {
                    IWeapon mObject = Instantiate<IWeapon>(healWeapon, targets[i].bottomTransform);

                    mObject.amount = moveAffectDuration;

                    mObject.moveDuration = MoveTurnsType;

                    mObject.Trigger();
                }

            })).Append(DOVirtual.DelayedCall(playTime, () => {

                player.transform.DORotateQuaternion(tempRotation, 0.3f);

            }));

            mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });
        }
    }
}
