using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu ( fileName = "Objects" , menuName = "ScriptableObjects/BATTLE CHOICES - ArcherOneShot" )]
public class ArcherOneShot : BattleChoice
{
    public IWeapon Bow;

    public IWeapon arrow;

    public override void MoveWork (List<BattlePlayer> target )
    {
        BattlePlayer tPlayer = BattleManager.Instance.currentPlayer;

        if ( tPlayer != null )
        {
            IWeapon m_Arrow = Instantiate<IWeapon> ( arrow , tPlayer.rightHandSpawnInside );

            IWeapon mObject = Instantiate<IWeapon> ( Bow , tPlayer.leftHandSpawnInside );

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger ( );

            tPlayer.transform.DOLookAt(target[0].transform.position, 0.1f);

            tPlayer.mPlayerController.SetTrigger ( m_AnimationClip.ToString ( ) );

            m_Arrow.Trigger ( target );

            foreach ( BattlePlayer m_Player in tPlayer.target )
            {
                MoveManager.Instance.CalculateDamage ( this , m_Player );
            }

            DOVirtual.DelayedCall(endTime, () => { base.MoveWork(null); });


            foreach (BattlePlayer player in target)
            {
                MoveManager.Instance.CalculateDamage(this, player);
            }
        }


    }
}

