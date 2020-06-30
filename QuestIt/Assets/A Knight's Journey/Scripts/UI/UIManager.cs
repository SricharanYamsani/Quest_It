using System.Collections;
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
    public RectTransform hudPanel;
    public RectTransform overlayPanel;

    [SerializeField] StreamVideo videoPlayer;

    [SerializeField]
    public RPG.Control.PlayerWorldController playerController;

    public Button playerCharacteristicsButton;

    public Image playerCharacteristicsIcon;

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

        //DontDestroyOnLoad(gameObject);
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

        //playerCharacteristicsIcon.sprite = GetPlayerImage(playerController.playerInfo.character);
    }

    void OnPlayerCharacteristicsButtonClicked()
    {
        if(playerCharacteristicsManager != null)
        {
            playerCharacteristicsManager.gameObject.SetActive(true);
        }
        else
        {
            playerCharacteristicsManagerPrefab.GetComponent<BgVideoPlayer>().videoPlayer = videoPlayer;
            playerCharacteristicsManager = Instantiate(playerCharacteristicsManagerPrefab, overlayPanel.transform);            
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

