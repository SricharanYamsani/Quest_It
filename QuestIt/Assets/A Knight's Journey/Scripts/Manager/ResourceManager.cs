using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using RPG.NPCs;
using RPG.Control;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<Moves, MoveChoice> GetChoiceFromMove = new Dictionary<Moves, MoveChoice>();

    public Dictionary<Consumables, ConsumableChoice> GetChoiceFromConsumables = new Dictionary<Consumables, ConsumableChoice>();

    public List<Sprite> choiceSprites = new List<Sprite>();

    public List<Sprite> currencySprites = new List<Sprite>();

    public Dictionary<string, Sprite> playerIcons = new Dictionary<string, Sprite>();

    /// <summary>Currency Sprites -> Gold. Mana. Health. </summary>
    public Dictionary<string, Sprite> currencySpritesRef = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> choiceSpritesRef = new Dictionary<string, Sprite>();

    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public Dictionary<string, AudioClip> bgClips = new Dictionary<string, AudioClip>();

    public Dictionary<BattleCharacters, BattlePlayer> allModels = new Dictionary<BattleCharacters, BattlePlayer>();

    public Dictionary<BattleCharacters, PlayerWorldController> playerMovement = new Dictionary<BattleCharacters, PlayerWorldController>();

    public Dictionary<string, IWeapon> effects = new Dictionary<string, IWeapon>();

    public Dictionary<string, GameObject> reactionObjects = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;

        base.Awake();

        MoveChoice[] choices = Resources.LoadAll<MoveChoice>("ScriptableObjects");

        ConsumableChoice[] consumables = Resources.LoadAll<ConsumableChoice>("ScriptableObjects");

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/SFX");

        AudioClip[] _clips = Resources.LoadAll<AudioClip>("Sounds/Background");

        BattlePlayer[] models = Resources.LoadAll<BattlePlayer>("Models");

        IWeapon[] p_Effects = Resources.LoadAll<IWeapon>("IWeapons");

        Sprite[] p_Icons = Resources.LoadAll<Sprite>("Sprites/PlayerIcons");

        GameObject[] reactions = Resources.LoadAll<GameObject>("Prefabs/ReactionObjects");

        PlayerWorldController[] controllers = Resources.LoadAll<PlayerWorldController>("Models/PlayerControllers");

        choiceSprites = Resources.LoadAll<Sprite>("Sprites/Choices").ToList();

        currencySprites = Resources.LoadAll<Sprite>("Sprites/Currency").ToList();

        foreach (PlayerWorldController controller in controllers)
        {
            if (!playerMovement.ContainsKey(controller.playerInfo.character))
            {
                playerMovement.Add(controller.playerInfo.character, controller);
            }
        }

        foreach (Sprite icon in p_Icons)
        {
            if (!playerIcons.ContainsKey(icon.name))
            {
                playerIcons.Add(icon.name, icon);
            }
        }

        for (int i = 0; i < p_Effects.Length; i++)
        {
            if (!effects.ContainsKey(p_Effects[i].name))
            {
                effects.Add(p_Effects[i].name, p_Effects[i]);
            }
        }

        for (int i = 0; i < choices.Length; i++)
        {
            if (!GetChoiceFromMove.ContainsKey(choices[i].move))
            {
                GetChoiceFromMove.Add(choices[i].move, choices[i]);
            }
        }

        for (int i = 0; i < consumables.Length; i++)
        {
            if (!GetChoiceFromConsumables.ContainsKey(consumables[i].itemName))
            {
                GetChoiceFromConsumables.Add(consumables[i].itemName, consumables[i]);
            }
        }

        for (int i = 0; i < clips.Length; i++) // Audio Clips Loading in Dictionary
        {
            if (!soundClips.ContainsKey(clips[i].name))
            {
                soundClips.Add(clips[i].name, clips[i]);
            }
        }

        for (int i = 0; i < _clips.Length; i++) // Audio Clips Loading in Dictionary
        {
            if (!soundClips.ContainsKey(_clips[i].name))
            {
                bgClips.Add(_clips[i].name, _clips[i]);
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

        for (int i = 0; i < reactions.Length; i++)
        {
            if (!reactionObjects.ContainsKey(reactions[i].name))
            {
                reactionObjects.Add(reactions[i].name, reactions[i]);
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
