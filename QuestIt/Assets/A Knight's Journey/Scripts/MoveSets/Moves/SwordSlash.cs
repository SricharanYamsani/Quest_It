using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BATTLE CHOICES - SWORD SLASH")]
public class SwordSlash : MoveChoice
{
    public IWeapon sword;

    public override void MoveWork(BattlePlayer player, List<BattlePlayer> target)
    {
        float timeTaken = 0;

        if (player != null)
        {
            IWeapon mObject = Instantiate<IWeapon>(sword, player.rightHandSpawnInside);

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            Sequence mySequence = DOTween.Sequence();

            Vector3 targetPos = target[0].transform.position;

            mySequence.Append(player.transform.DOLookAt(targetPos, 0.4f));

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                float dist = Mathf.Abs(player.transform.position.z - target[0].meleeAttackSpawn.position.z);

                float speed = 1.5f;

                float tTime = dist / speed;

                player.mPlayerController.SetBool("WALK", true);

                timeTaken = 0.25f;

                timeTaken += tTime;

                player.transform.DOMoveZ(target[0].meleeAttackSpawn.position.z, tTime).OnComplete(() =>
                {
                    player.mPlayerController.SetBool("WALK", false);

                    player.PlayAnimation(m_AnimationClip.ToString());

                    mObject.Trigger(target);

                });


            })).AppendInterval(endTime - timeTaken);

            mySequence.Append(player.transform.DOLookAt(player.OriginalSpawn.position, 0.4f));

            mySequence.Append(DOVirtual.DelayedCall(0.25f, () =>
            {
                timeTaken = 0;

                float dist = Mathf.Abs(player.transform.position.z - player.OriginalSpawn.position.z);

                float speed = 1.5f;

                float tTime = dist / speed;

                timeTaken = 0.25f + tTime + 0.3f;

                player.mPlayerController.SetBool("WALK", true);

                player.transform.DOMoveZ(player.OriginalSpawn.position.z, tTime).OnComplete(() =>
                {
                    player.mPlayerController.SetBool("WALK", false);

                    player.transform.DOLookAt(targetPos, 0.3f);

                });
            }));

            mySequence.AppendInterval(timeTaken);

                mySequence.OnComplete(() =>
            {
                base.MoveWork(player, null);
            });
        }
    }
}
