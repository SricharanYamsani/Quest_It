using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string , BattleChoice> GetChoiceForString = new Dictionary<string , BattleChoice> ( );

    public List<Sprite> choiceSprites = new List<Sprite> ( );

    public List<Sprite> currencySprites = new List<Sprite> ( );

    public Dictionary<string , Sprite> currencySpritesRef = new Dictionary<string , Sprite> ( );

    public Dictionary<string , Sprite> choiceSpritesRef = new Dictionary<string , Sprite> ( );

    protected override void Awake ( )
    {
        base.Awake ( );

        BattleChoice [ ] choices = Resources.LoadAll<BattleChoice> ( "ScriptableObjects" );

        for ( int i = 0 ; i < choices.Length ; i++ )
        {
            if ( !GetChoiceForString.ContainsKey ( choices [ i ].name ) )
            {
                GetChoiceForString.Add ( choices [ i ].name , choices [ i ] );
            }
        }

        for ( int i = 0 ; i < choiceSprites.Count ; i++ )
        {
            if ( !choiceSpritesRef.ContainsKey ( choiceSprites [ i ].name ) )
            {
                choiceSpritesRef.Add ( choiceSprites [ i ].name , choiceSprites [ i ] );
            }
        }
        for ( int i = 0 ; i < currencySprites.Count ; i++ )
        {
            if ( !currencySpritesRef.ContainsKey ( currencySprites [ i ].name ) )
            {
                currencySpritesRef.Add ( currencySprites [ i ].name , currencySprites [ i ] );
            }
        }
    }
}
