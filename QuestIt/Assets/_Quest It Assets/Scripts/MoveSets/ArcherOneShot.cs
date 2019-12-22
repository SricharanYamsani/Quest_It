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

            BattlePlayer enemy = tPlayer.target [ 0 ];

            if(enemy == null)
            {
                Debug.LogError ( "NULL ENEMY" );
            }
            else
            {
                Debug.LogWarning ( enemy.name );
            }
            List<BattlePlayer> mPlayerList = new List<BattlePlayer> ( );

            mPlayerList.Add ( enemy );

            mObject.amount = moveAffectDuration;

            mObject.moveDuration = MoveTurnsType;

            mObject.Trigger ( );

            tPlayer.mPlayerController.SetTrigger ( m_AnimationClip.ToString ( ) );

            m_Arrow.Trigger ( mPlayerList );

            foreach ( BattlePlayer m_Player in tPlayer.target )
            {
                MoveManager.Instance.CalculateDamage ( this , m_Player );
            }
        }
    }
}

