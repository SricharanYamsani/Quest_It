using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<Moves, MoveChoice> GetChoiceFromMove = new Dictionary<Moves, MoveChoice>();

    public List<Sprite> choiceSprites = new List<Sprite>();

    public List<Sprite> currencySprites = new List<Sprite>();

    /// <summary>Currency Sprites -> Gold. Mana. Health. </summary>
    public Dictionary<string, Sprite> currencySpritesRef = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> choiceSpritesRef = new Dictionary<string, Sprite>();

    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public Dictionary<BattleCharacters, BattlePlayer> allModels = new Dictionary<BattleCharacters, BattlePlayer>();

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;

        base.Awake();

        MoveChoice[] choices = Resources.LoadAll<MoveChoice>("ScriptableObjects");

        ConsumableChoice[] consumables = Resources.LoadAll<ConsumableChoice>("ScriptableObjects");

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        BattlePlayer[] models = Resources.LoadAll<BattlePlayer>("Models");

        choiceSprites = Resources.LoadAll<Sprite>("Sprites/Choices").ToList();

        currencySprites = Resources.LoadAll<Sprite>("Sprites/Currency").ToList();

        for (int i = 0; i < choices.Length; i++)
        {
            Moves move;

            Enum.TryParse<Moves>(choices[i].name, out move);

            if (move != Moves.NONE)
            {

                if (!GetChoiceFromMove.ContainsKey(move))
                {
                    GetChoiceFromMove.Add(move, choices[i]);
                }
            }
        }

        for (int i = 0; i < clips.Length; i++) // Audio Clips Loading in Dictionary
        {
            if (!soundClips.ContainsKey(clips[i].name))
            {
                soundClips.Add(clips[i].name, clips[i]);
            }
        }

        for (int i = 0; i < choiceSprites.Count; i++)
        {
            if (!choiceSpritesRef.ContainsKey(choiceSprites[i].name))
            {
                choiceSpritesRef.Add(choiceSprites[i].name, choiceSprites[i]);
            }
        }
        for (int i = 0; i < currencySprites.Count; i++)
        {
            if (!currencySpritesRef.ContainsKey(currencySprites[i].name))
            {
                currencySpritesRef.Add(currencySprites[i].name, currencySprites[i]);
            }
        }

        for (int i = 0; i < models.Length; i++)
        {
            BattleCharacters character;

            Enum.TryParse<BattleCharacters>(models[i].name, out character);

            if (character != BattleCharacters.NONE)
            {
                if (!allModels.ContainsKey(character))
                {
                    allModels.Add(character, models[i]);
                }
            }
        }
    }
}
