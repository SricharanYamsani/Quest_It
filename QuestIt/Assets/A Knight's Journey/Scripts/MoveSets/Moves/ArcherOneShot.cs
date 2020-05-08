using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - ArcherOneShot")]
public class ArcherOneShot : MoveChoice
{
    public IWeapon Bow;

    public IWeapon arrow;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> target)
    {
        if (player != null)
        {
            IWeapon mObject = Instantiate<IWeapon>(Bow, player.leftHandSpawnInside);

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger();

            player.transform.DOLookAt(target[0].transform.position, 0.1f).OnComplete(() =>
            {
                foreach (BattlePlayer myPlayer in target)
                {
                    IWeapon m_Arrow = Instantiate<IWeapon>(arrow, player.rightHandSpawnInside);

                    m_Arrow.Trigger(myPlayer);
                }
            });

            player.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            DOVirtual.DelayedCall(endTime, () => { base.MoveWork(null, null); });
        }
    }
}
