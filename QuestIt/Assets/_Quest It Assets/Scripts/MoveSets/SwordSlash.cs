using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - SWORD SLASH")]
public class SwordSlash : BattleChoice
{
    public IWeapon sword;

    public override void MoveWork(List<BattlePlayer> target)
    {
        BattlePlayer tPlayer = BattleManager.Instance.currentPlayer;

        if (tPlayer != null)
        {
            IWeapon mObject = Instantiate<IWeapon>(sword, tPlayer.rightHandSpawnInside);

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger();

            List<BattlePlayer> mPlayers = target;

            tPlayer.mPlayerController.SetBool("Walking", true);

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(tPlayer.transform.DOMove((mPlayers[0].meleeAttackSpawn.position), 1.25f)).Append(DOVirtual.DelayedCall(0, () =>
            {
                tPlayer.mPlayerController.SetBool("Walking", false);

                tPlayer.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(endTime, () => { tPlayer.transform.DOMove(tPlayer.OriginalSpawn.position, 1.25f); }));

            mySequence.OnUpdate(() => { tPlayer.transform.LookAt(mPlayers[0].transform.position); });

            mySequence.OnComplete(() => { base.MoveWork(null); });

            foreach (BattlePlayer m_Player in mPlayers)
            {
                MoveManager.Instance.CalculateDamage(this, m_Player);
            }
        }
    }
}

