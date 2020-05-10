using System.Collections;
using System.Collections.Generic;
using System;

public static class PlayerInfoResource
{
    public static List<Moves> GetPlayerMoves(BattleCharacters character)
    {
        List<Moves> characterMoves = new List<Moves>();

        switch (character)
        {
            case BattleCharacters.COMBATANT:
                characterMoves.Add(Moves.LIGHTNING_SMALL_1);
                characterMoves.Add(Moves.LIGHTNING_SMALL_ENEMY);
                characterMoves.Add(Moves.LIGHNING_MID_1);
                characterMoves.Add(Moves.LIGHNING_MID_ENEMY);
                break;
            case BattleCharacters.OCCULTIST:
                characterMoves.Add(Moves.LIGHNING_MID_ENEMY);
                break;
        }
        return characterMoves;
    }
}
