using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAttributes
{
    public int maxHealth;

    public int curHealth;

    public int maxAgility;

    public int curAgility;

    public int maxDefense;

    public int curDefense;

    public int maxLuck;

    public int curLuck;

    public int GetCurrentHealth ( ) {

        return curHealth;
    }
}
