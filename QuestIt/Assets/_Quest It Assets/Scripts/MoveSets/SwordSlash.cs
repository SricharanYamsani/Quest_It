using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - SWORD SLASH")]
public class SwordSlash : BattleChoice
{
    public IWeapon sword;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> target)
    {
        if (player != null)
        {
            IWeapon mObject = Instantiate<IWeapon>(sword, player.rightHandSpawnInside);

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger();

            List<BattlePlayer> mPlayers = target;

            Sequence mySequence = DOTween.Sequence();

            float calculatedTime = Vector3.Distance(player.transform.position, mPlayers[0].meleeAttackSpawn.transform.position) / 2;

            Debug.Log(calculatedTime);

            float finishingTime = endTime + calculatedTime;

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {

                player.transform.DOLookAt(target[0].transform.position, 0.4f);

                player.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(endTime, () => {; }));

            mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });

            foreach (BattlePlayer m_Player in mPlayers)
            {
                MoveManager.Instance.CalculateDamage(player, this, m_Player);
            }
        }
    }
}
