using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string , BattleChoice> GetChoiceForString = new Dictionary<string , BattleChoice> ( );

    public List<Sprite> choiceSprites = new List<Sprite> ( ); // Currently Assigned.

    public List<Sprite> currencySprites = new List<Sprite> ( );// Currently Assigned.

    /// <summary>Currency Sprites -> Gold. Mana. Health. </summary>
    public Dictionary<string , Sprite> currencySpritesRef = new Dictionary<string , Sprite> ( );

    public Dictionary<string , Sprite> choiceSpritesRef = new Dictionary<string , Sprite> ( );

    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    protected override void Awake ( )
    {
        base.Awake ( );

        BattleChoice [ ] choices = Resources.LoadAll<BattleChoice> ( "ScriptableObjects" );

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        for ( int i = 0 ; i < choices.Length ; i++ )
        {
            if ( !GetChoiceForString.ContainsKey ( choices [ i ].name ) )
            {
                GetChoiceForString.Add ( choices [ i ].name , choices [ i ] );
            }
        }

        for (int i = 0; i < clips.Length; i++) // Audio Clips Loading in Dictionary
        {
            if (!soundClips.ContainsKey(clips[i].name))
            {
                soundClips.Add(clips[i].name, clips[i]);
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
