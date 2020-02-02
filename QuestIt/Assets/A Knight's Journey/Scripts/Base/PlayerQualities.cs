using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerQualities
{
    public PlayerAttributes myAttributes = new PlayerAttributes();

    public List<MOVES> chosenMoves = new List<MOVES>();

    public BattleCharacters character = BattleCharacters.NONE;

    public bool IsTeamRed { get; set; }

    public bool IsPlayer { get; set; }
}
