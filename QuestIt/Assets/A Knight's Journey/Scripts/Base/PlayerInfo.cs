using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerInfo
{
    public PlayerAttributes myAttributes = new PlayerAttributes();

    public List<Moves> chosenMoves = new List<Moves>();

    public List<ConsumablesInfo> consumables = new List<ConsumablesInfo>();

    public BattleCharacters character = BattleCharacters.NONE;

    public Elemental playerElements = Elemental.NONE;

    public bool IsTeamRed { get; set; }

    public bool IsPlayer { get; set; }

    public int experience = 0;

    public int GetLevel()
    {
        return ExperienceToLevel();
    }

    private int ExperienceToLevel()
    {
        int temp = experience;

        int levelIndex = 0;

        while (temp > 0)
        {
            temp -= (100 + (levelIndex > 0 ? (int)(Math.Pow(200, levelIndex)) : 0));

            if (temp > 0)
            {
                levelIndex++;
            }
        }

        return levelIndex;
    }
}
