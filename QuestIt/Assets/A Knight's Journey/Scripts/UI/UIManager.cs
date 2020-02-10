using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private UIManager instance;
    public UIManager Instance
    {
        get { return instance; }
    }

    public Canvas mainCanvas;
    public RectTransform HUDPanel;
    public RectTransform OverlayPanel;

    public Button playerCharacteristicsButton;

    [HideInInspector]public PlayerCharacteristicsManager playerCharacteristicsManager;
    public PlayerCharacteristicsManager playerCharacteristicsManagerPrefab;

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

}

