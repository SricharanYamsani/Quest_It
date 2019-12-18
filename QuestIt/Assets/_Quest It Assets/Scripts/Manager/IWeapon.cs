using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour
{
    public int Irounds = 0;

    public MoveDuration moveDuration;

    public int amount;

    public void Awake ( )
    {
        Irounds = 0;
    }

    public void Trigger ( )
    {
        if(moveDuration == MoveDuration.ROUNDS)
        {
            BattleManager.Instance.RoundOver += RoundSystems;
        }
        else if(moveDuration == MoveDuration.TURNS)
        {
            BattleManager.Instance.TurnOver += RoundSystems;
        }
    }

    public virtual void RoundSystems ( )
    {
        Irounds++;

        if ( Irounds > amount )
        {
            Destroy ( this.gameObject );
        }
    }

    private void OnDestroy ( )
    {
        BattleManager.Instance.RoundOver -= RoundSystems;

        BattleManager.Instance.TurnOver -= RoundSystems;
    }
}
