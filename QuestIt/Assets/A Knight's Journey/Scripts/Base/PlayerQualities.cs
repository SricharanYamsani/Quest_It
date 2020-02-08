using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerQualities
{
    public PlayerAttributes myAttributes = new PlayerAttributes();

    public List<Moves> chosenMoves = new List<Moves>();

    public BattleCharacters character = BattleCharacters.NONE;

    public bool IsTeamRed { get; set; }

    public bool IsPlayer { get; set; }
}
