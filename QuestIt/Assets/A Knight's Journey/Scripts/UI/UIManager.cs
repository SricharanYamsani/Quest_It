﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public Canvas mainCanvas;
    public RectTransform HUDPanel;
    public RectTransform OverlayPanel;

    [SerializeField]
    public RPG.Control.PlayerWorldController playerController;

    public Button playerCharacteristicsButton;

    [HideInInspector]public PlayerCharacteristicsManager playerCharacteristicsManager;
    public PlayerCharacteristicsManager playerCharacteristicsManagerPrefab;

    [EnumNamedArray(typeof(BattleCharacters))]
    public List<Sprite> playerIcons;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupButtons();
    }

    void Update()
    {
        
    }

    void SetupButtons()
    {
        playerCharacteristicsButton.onClick.RemoveAllListeners();

        playerCharacteristicsButton.onClick.AddListener(OnPlayerCharacteristicsButtonClicked);
    }

    void OnPlayerCharacteristicsButtonClicked()
    {
        if(playerCharacteristicsManager != null)
        {
            playerCharacteristicsManager.gameObject.SetActive(true);
        }
        else
        {
            playerCharacteristicsManager = Instantiate(playerCharacteristicsManagerPrefab, OverlayPanel.transform);
        }
    }

    public Sprite GetPlayerImage(BattleCharacters character)
    {
        switch (character)
        {
            case BattleCharacters.FOOL:
            case BattleCharacters.OCCULTIST:
            case BattleCharacters.COMBATANT:
            case BattleCharacters.GUARDIAN:
            case BattleCharacters.TOXOPHILITE:
                return playerIcons[(int)character];
        }
        return null;
    }

}

