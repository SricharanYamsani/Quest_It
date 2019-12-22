using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu ( fileName = "Objects" , menuName = "ScriptableObjects/BATTLE CHOICES - SWORD SLASH" )]
public class SwordSlash : BattleChoice
{
    public IWeapon sword;

    public override void MoveWork ( )
    {
        BattlePlayer tPlayer = BattleManager.Instance.currentPlayer;

        if ( tPlayer != null )
        {
            IWeapon mObject = Instantiate<IWeapon> ( sword , tPlayer.rightHandSpawnInside );

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger ( );

            tPlayer.mPlayerController.SetBool ( "Walking" , true );

            tPlayer.transform.DOMove ( new Vector3 ( tPlayer.meleeAttackSpawn.position.x ,tPlayer.transform.position.y ,tPlayer.target[0].meleeAttackSpawn.position.z ) , 1.25f ).OnComplete ( ( ) =>
                   {
                       tPlayer.mPlayerController.SetBool ( "Walking" , false );

                       tPlayer.mPlayerController.SetTrigger ( m_AnimationClip.ToString ( ) );

                       Debug.Log ( tPlayer.mPlayerController.GetCurrentAnimatorClipInfo ( 0 ).Length );// + 2f;

                       Debug.Log ( endTime );
                   } );

            foreach(BattlePlayer m_Player in tPlayer.target)
            {
                MoveManager.Instance.CalculateDamage ( this , m_Player );
            }
        }
    }
}

