using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BattleChoice
{
    public float endTime = 5f;

    public abstract void mWork ( );
}
