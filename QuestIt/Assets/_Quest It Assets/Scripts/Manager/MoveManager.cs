using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : Singleton<MoveManager>
{
    protected override void Awake ( )
    {
        base.Awake ( );
    }

    public void SetMoveDuration(MoveDuration mDuration, int rounds)
    {
        if(mDuration == MoveDuration.TURNS)
        {
            // Affect for turns
        }
        else if(mDuration == MoveDuration.ROUNDS)
        {
            // Affect for Rounds
        }
        else if(mDuration == MoveDuration.NONE)
        {
            // Nothing to happen
        }
    }
}

public enum MoveDuration
{
    NONE,
    ROUNDS,
    TURNS
}
