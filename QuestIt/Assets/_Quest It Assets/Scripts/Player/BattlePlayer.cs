using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    public PlayerAttributes attributes = new PlayerAttributes ( );

    public List<BattleChoice> validChoices = new List<BattleChoice> ( );

    public BattleChoice currentChoice = null;

    public Transform WorldUI = null;

    private void LateUpdate ( )
    {
        if ( WorldUI )
        {
            WorldUI.transform.position = transform.position;
        }
    }

}
