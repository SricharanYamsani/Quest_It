using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string , BattleChoice> GetChoiceForString = new Dictionary<string , BattleChoice> ( );

    protected override void Awake ( )
    {
        base.Awake ( );

        BattleChoice [ ] choices = Resources.LoadAll<BattleChoice> ( "ScriptableObjects" );

        for ( int i = 0 ; i < choices.Length ; i++ )
        {
            if(!GetChoiceForString.ContainsKey(choices[i].name))
            {
                GetChoiceForString.Add ( choices [ i ].name , choices [ i ] );
            }
        }
    }
}
