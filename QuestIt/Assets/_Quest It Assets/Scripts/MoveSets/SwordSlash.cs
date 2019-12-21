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

        Debug.Log ( "Player : " );

        if ( tPlayer != null )
        {
            IWeapon mObject = Instantiate<IWeapon> ( sword , tPlayer.rightHandSpawnInside );

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger ( );

            tPlayer.mPlayerController.SetBool ( "Walking" , true );

            float posZ = tPlayer.isPlayer ? -1.5f : 1.5f;

            tPlayer.transform.DOMove ( new Vector3 ( tPlayer.transform.position.x , tPlayer.transform.position.y , posZ ) , 0.5f ).OnComplete ( ( ) =>
                   {
                       tPlayer.mPlayerController.SetBool ( "Walking" , false );

                       tPlayer.mPlayerController.SetTrigger ( m_AnimationClip.ToString ( ) );
                   } );
        }
    }
}

