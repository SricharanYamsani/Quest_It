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

            Sequence mySequence = DOTween.Sequence();

            float calculatedTime = Vector3.Distance(tPlayer.transform.position, mPlayers[0].meleeAttackSpawn.transform.position) / 2;

            Debug.Log(calculatedTime);

            float finishingTime = endTime + calculatedTime;

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {

                tPlayer.transform.DOLookAt(target[0].transform.position, 0.4f);

                tPlayer.mPlayerController.SetTrigger(m_AnimationClip.ToString());

            })).Append(DOVirtual.DelayedCall(endTime, () => {; }));
//            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
//            {
//                tPlayer.transform.DOMove((mPlayers[0].meleeAttackSpawn.position), calculatedTime).OnComplete(() =>
//                {
//                    tPlayer.mPlayerController.SetBool("Walking", false);

//                    tPlayer.mPlayerController.SetTrigger(m_AnimationClip.ToString());
//                })

//.OnUpdate((() => { tPlayer.transform.LookAt(mPlayers[0].transform); }));
//            }))

//               .Append(DOVirtual.DelayedCall(finishingTime, () =>
//               {

//                   tPlayer.mPlayerController.SetBool("Walking", true); tPlayer.transform.DOMove(tPlayer.OriginalSpawn.position, calculatedTime).OnComplete(() =>
//                   {

//                       tPlayer.mPlayerController.SetBool("Walking", false);

//                   });
//               }));

            mySequence.OnComplete(() =>
            { 
                base.MoveWork(null);
            });

            foreach (BattlePlayer m_Player in mPlayers)
            {
                MoveManager.Instance.CalculateDamage(this, m_Player);
            }
        }
    }
}
