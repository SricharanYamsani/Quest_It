using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : IWeapon
{
    public override void Trigger (List<BattlePlayer> targets = null)
    {
        if ( targets != null )
        {
            if ( targets.Count > 0 )
            {
                Debug.Log ( targets.Count );

                BattlePlayer target = targets [ 0 ];

                Debug.Log ( target.name );

                Debug.Log ( this.name );

                DOVirtual.DelayedCall ( 3.15f , ( ) =>
                {
                    transform.DOMove ( target.torsoTransform.position , 0.5f ).OnComplete ( ( ) =>
                    {
                        target.ShowReaction ( );

                        RoundSystems ( );
                    } );
                } );
            }
            else
            {
                Debug.LogWarning ( "Count Error" );
            }
        }
        else
        {
            Debug.LogWarning ( "NULL ERROR" );
        }
    }
    public override void RoundSystems ( )
    {
        Destroy ( gameObject );
    }
}
