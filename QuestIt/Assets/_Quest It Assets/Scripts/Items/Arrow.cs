using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : IWeapon
{
    public override void Trigger ( )
    {
        BattlePlayer target = BattleManager.Instance.currentPlayer.target [ 0 ];

        this.transform.DOMove ( target.torsoTransform.position , 0.5f ).OnComplete ( ( ) => { target.ShowReaction ( ); RoundSystems ( ); } );
    }

    public override void RoundSystems ( )
    {
        Destroy ( this.gameObject );
    }
}
