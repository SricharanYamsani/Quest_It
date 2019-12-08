using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( fileName = "Objects" , menuName = "ScriptableObjects/BATTLE CHOICES - ATTACK" )]
public class ATTACK : BattleChoice
{
    public override void mWork ( BattlePlayer mPlayer)
    {
        mPlayer.Health ( -healthChange );

        Debug.Log ( typeof ( Component ).Name );
    }
}
