using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleData 
{
    public string BattleID;

    public BattleOutcome Outcome;

    public List<BattleCharacters> BattleWinners = new List<BattleCharacters>();

    public List<BattleCharacters> BattleLosers = new List<BattleCharacters>();

    public List<Moves> movesPlayed = new List<Moves>();

    public List<Consumables> consumablesUsed = new List<Consumables>();

    public int RoundsPlayed;

    public int TurnsPlayed;
}
