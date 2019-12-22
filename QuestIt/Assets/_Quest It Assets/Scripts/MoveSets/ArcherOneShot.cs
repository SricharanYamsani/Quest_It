using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu ( fileName = "Objects" , menuName = "ScriptableObjects/BATTLE CHOICES - ArcherOneShot" )]
public class ArcherOneShot : BattleChoice
{
    public IWeapon Bow;

    public IWeapon arrow;

    public override void MoveWork ( )
    {
        BattlePlayer tPlayer = BattleManager.Instance.currentPlayer;

        if ( tPlayer != null )
        {
            IWeapon m_Arrow = Instantiate<IWeapon> ( arrow , tPlayer.rightHandSpawnInside );

            IWeapon mObject = Instantiate<IWeapon> ( Bow , tPlayer.leftHandSpawnInside );

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger ( );

            if(m_Arrow)
            {
                Debug.Log("Spawned");
            }

            tPlayer.mPlayerController.SetBool ( "Walking" , true );

            tPlayer.transform.DOMove ( new Vector3 ( tPlayer.meleeAttackSpawn.position.x ,tPlayer.transform.position.y ,tPlayer.meleeAttackSpawn.position.z ) , 0.5f ).OnComplete ( ( ) =>
                   {
                       tPlayer.mPlayerController.SetBool ( "Walking" , false );

                       tPlayer.mPlayerController.SetTrigger ( m_AnimationClip.ToString ( ) );

                       Debug.Log ( tPlayer.mPlayerController.GetCurrentAnimatorClipInfo ( 0 ).Length );// + 2f;

                       m_Arrow.Trigger ( );

                   } );

            foreach(BattlePlayer m_Player in tPlayer.target)
            {
                MoveManager.Instance.CalculateDamage ( this , m_Player );
            }
        }
    }
}

