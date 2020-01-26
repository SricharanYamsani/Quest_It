using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAttributes
{
    public Attributes health = new Attributes();

    public Attributes mana = new Attributes();

    public Attributes attack = new Attributes();

    public Attributes defense = new Attributes();

    public Attributes luck = new Attributes();

    public Attributes agility = new Attributes();

    public Attributes regenerationMana = new Attributes();

    public Attributes regenerationHealth = new Attributes();
}

[Serializable]
public class Attributes
{
    public int maximum;

    public int current;

    public Attributes()
    {

    }

    public Attributes(int max, int cur)
    {
        maximum = max;
        current = cur;
    }
}
[Serializable]
public struct MaxAttributes
{
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Agility { get; set; }
    public int Luck { get; set; }
}

