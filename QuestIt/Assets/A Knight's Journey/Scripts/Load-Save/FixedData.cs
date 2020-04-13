using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo info = new PlayerInfo();

        info.character = BattleCharacters.OCCULTIST;

        info.myAttributes = new PlayerAttributes();

        info.myAttributes.health.maximum = 100;

        info.myAttributes.mana.maximum = 60;

        info.myAttributes.luck.maximum = 30;

        info.myAttributes.defense.maximum = 80;

        info.chosenMoves = new List<Moves>
        {
            Moves.BRING_DOWN_FIRE_1,
            Moves.BRING_DOWN_FIRE_ENEMY,
            Moves.LIGHNING_MAX_1,
            Moves.LIGHNING_MID_1,
            Moves.LIGHNING_MID_TEAM,
            Moves.LIGHTNING_MAX_TEAM,
            Moves.MAGIC_HEAL_MID_1
        };

        SavingClass s = new SavingClass();

        s.entities.Add("characters", info);

        if (SaveManager.SaveResourceJson(info,"Occultist"))
        {
            
        }
    }
}

[System.Serializable]
public class SavingClass
{
    public Dictionary<string, object> entities = new Dictionary<string, object>();
}