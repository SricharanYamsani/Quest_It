using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "OBJECTS", menuName = "SCRIPTABlE OBJECTS/MELEE DAMAGE")]
public class MeleeAttack : MoveChoice
{
    public IWeapon slashAttack;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> target)
    {
        float timeTaken = 0;

        if (player != null)
        {
            Sequence mySequence = DOTween.Sequence();

            Vector3 targetPos = target[0].transform.position;

            mySequence.Append(player.transform.DOLookAt(targetPos, 0.4f));

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                player.PlayAnimation(m_AnimationClip.ToString());

            }));

            mySequence.AppendInterval(1.25f);

            mySequence.Append(DOVirtual.DelayedCall(0, () =>
            {
                for (int i = 0; i < target.Count; i++)
                {
                    IWeapon mObject = Instantiate<IWeapon>(slashAttack, target[i].torsoTransform);

                    mObject.amount = moveAffectDuration;

                    mObject.moveDuration = MoveTurnsType;

                    mObject.Trigger();
                }
            }));

            mySequence.Append(player.transform.DOLookAt(targetPos, 0.3f));

            mySequence.AppendInterval(timeTaken);

            mySequence.OnComplete(() =>
        {
            
            base.MoveWork(player, null);
        });
        }
    }
}
