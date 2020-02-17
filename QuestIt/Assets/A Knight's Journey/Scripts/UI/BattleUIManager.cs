using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class BattleUIManager : Singleton<BattleUIManager>
{
    #region ObjectReferences

    public RectTransform playerHUD;
    public RectTransform enemyHUD;


    // Selection Panel
    public RectTransform selectionPanel;

    public PlayerIcon playerIconRef;
    public PlayerIcon enemyIconRef;

    public CanvasGroup RadialButton;
    public GameObject gameOverScreen;

    public Button selectTrueButton;

    // Grids of Items
    public GameObject gridAttack;
    public GameObject gridItem;
    public GameObject gridDefend;
    public GameObject gridRun;

    private GameObject currentGrid;

    //Selection
    public GameObject selectionBackground;
    public SelectionBox selectionBox;
    public TacticsSelection tacticSelection = null;

    public List<TargetSelection> selectors = new List<TargetSelection>();
    #endregion
    public TextMeshProUGUI mText;

    #region Constants
    public const float fadeTimeRadial = 0.4f;
    #endregion
    bool isSliderOn = false;


    private float sizeXSlider = 0.0f;

    [SerializeField]
    List<BaseUIMove> t_Choices = new List<BaseUIMove>();
    [SerializeField]
    List<BaseUIConsumables> c_Choices = new List<BaseUIConsumables>();

    protected override void Awake()
    {
        base.Awake();

        CheckForSetup();

        gameOverScreen.SetActive(false);

        BattleManager.Instance.GameInit += LoadAllPlayerUI;

        BattleManager.Instance.GameOver += GameOverScreen;

        BattleManager.Instance.TurnStart += TurnStart;

        BattleManager.Instance.RoundOver += RoundOver;

        ChoiceManager.Instance.OnChoiceSelectionCompleted += SelectPlayerGo;

        //selectTrueButton.onClick.AddListener(() => { SelectPlayerGo(); });
    }
    private void CheckForSetup()
    {
        if(selectionPanel.gameObject.activeInHierarchy)
        {
            selectionPanel.gameObject.SetActive(false);
        }

        if(RadialButton.gameObject.activeInHierarchy)
        {
            RadialButton.gameObject.SetActive(false);
        }
    }

    private void RoundOver()
    {
       // Do stuff here for round Over
    }

    private void TurnStart(BattlePlayer obj)
    {
       
    }

    public void CloseButton()
    {
        if (RadialButton.gameObject.activeInHierarchy)
        {
            DOVirtual.DelayedCall(0.4f, () => { RadialButton.interactable = false; ShowRadialButton(false); });

            MoveOptionsLayer(false);

        }
    }

    private void LoadAllPlayerUI()
    {
        foreach (BattlePlayer player in BattleManager.Instance.GetAllPlayers())
        {
            RectTransform parentContent = null;

            PlayerIcon iconRef;

            if (player.IsTeamRed)
            {
                parentContent = playerHUD;
                iconRef = Instantiate<PlayerIcon>(playerIconRef, parentContent);
            }
            else
            {
                parentContent = enemyHUD;
                iconRef = Instantiate<PlayerIcon>(enemyIconRef, parentContent);
            }

            iconRef.Setup(player);

            player.playerIcon = iconRef;

            if (player.IsPlayer)
            {
                for (int i = 0; i < t_Choices.Count; i++)
                {
                    if (i < player.validChoices.Count)
                    {
                        t_Choices[i].m_Choice = player.validChoices[i];
                    }
                    else
                    {
                        t_Choices[i].m_Choice = null
;
                    }

                    t_Choices[i].SetupChoice();
                }

                for (int i = 0; i < c_Choices.Count; i++)
                {
                    if (i < player.playerInfo.consumables.Count)
                    {
                        ConsumablesInfo choice = player.playerInfo.consumables[i];
                        if (choice.amount > 0)
                        {
                            c_Choices[i].m_Choice = ResourceManager.Instance.GetChoiceFromConsumables[choice.consumable];

                            c_Choices[i].SetupChoice(choice.amount);
                        }
                    }
                }
            }
        }
    }

    public void ShowChoices(bool isTrue, TypesOfChoices buttonElement = TypesOfChoices.NONE)
    {
        if (isTrue)
        {
            switch (buttonElement)
            {
                case TypesOfChoices.ATTACK:
                                        SwitchGrids(0);
                    break;
                case TypesOfChoices.DEFEND: SwitchGrids(1); break;
                case TypesOfChoices.CONSUMABLES: SwitchGrids(2); break;
                case TypesOfChoices.RUN: SwitchGrids(3); break;
            }
        }

        MoveOptionsLayer(isTrue);
    }

    #region TargetChoices
    public void ShowTargetChoices(BattleChoice choice)
    {
        currentGrid.SetActive(false);

        bool canSelect = false;

        List<BattlePlayer> myTargets = BattleManager.Instance.GetTargetPlayers(choice, ref canSelect);

        selectionBox.LoadOptions(myTargets, canSelect);
    }
    #endregion

    public void SelectPlayerGo(List<BattlePlayer> targets)
    {
        if (BattleManager.Instance.currentPlayer.IsPlayer)
        {
            MoveOptionsLayer(false);

            DOVirtual.DelayedCall(0.4f, () => { ShowRadialButton(false); });
        }
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    private void MoveOptionsLayer(bool isTrue)
    {
        if (isTrue)
        {
            selectionPanel.gameObject.SetActive(true);

            selectionPanel.sizeDelta = new Vector2(0, selectionPanel.sizeDelta.y);

            selectionPanel.DOSizeDelta(new Vector2(1600, selectionPanel.sizeDelta.y), 0.4f).OnComplete(() =>
            {
                selectionPanel.sizeDelta = new Vector2(1600, selectionPanel.sizeDelta.y);

            });
        }
        else
        {
            selectionPanel.sizeDelta = new Vector2(1600, selectionPanel.sizeDelta.y);

            selectionPanel.DOSizeDelta(new Vector2(0, selectionPanel.sizeDelta.y), 0.4f).OnComplete(() =>
            {
                selectionPanel.gameObject.SetActive(false);
                selectionPanel.sizeDelta = new Vector2(0, selectionPanel.sizeDelta.y);
            });
        }
    }

    public void ShowRadialButton(bool isTrue)
    {
        if (isTrue)
        {
            RadialButton.interactable = false;

            RadialButton.gameObject.SetActive(true);

            RadialButton.DOFade(1, 0.4f).OnComplete(() => { RadialButton.interactable = true; });
        }
        else
        {
            RadialButton.interactable = false;

            RadialButton.DOFade(0, 0.4f).OnComplete(() => { RadialButton.gameObject.SetActive(false); });
        }
    }

    private void LoadValidMovesUI()
    {
        BattlePlayer currentPlayer = BattleManager.Instance.currentPlayer;

        if (currentPlayer.IsPlayer)
        {
            for (int i = 0; i < t_Choices.Count; i++)
            {
                bool open = false;

                if (t_Choices[i].m_Choice != null)
                {
                    if (t_Choices[i].m_Choice.m_Currency == Currency.BRUTE)
                    {
                        if (currentPlayer.CurrentHealth - t_Choices[i].m_Choice.m_CurrencyAmount > 1)
                        {
                            open = true;
                        }
                    }
                    else if (t_Choices[i].m_Choice.m_Currency == Currency.MANA)
                    {
                        if (currentPlayer.CurrentMana - t_Choices[i].m_Choice.m_CurrencyAmount > 1)
                        {
                            open = true;
                        }
                    }
                }
                else
                {

                }

                t_Choices[i].gameObject.SetActive(open);
            }
        }
    }
    public void SwitchGrids(int index)
    {
        // Sricharan - To Fix
        gridAttack.SetActive(false);
        gridDefend.SetActive(false);
        gridItem.SetActive(false);
        gridRun.SetActive(false);

        if (index == 0)
        {
            LoadValidMovesUI();
            sizeXSlider = BattleManager.Instance.currentPlayer.validChoices.Count * 200f;
            currentGrid = gridAttack;
        }
        else if (index == 1)
        {
            tacticSelection.OpenTacticsSelection();
            currentGrid = gridDefend;
        }
        else if (index == 2)
        {
            currentGrid = gridItem;
        }
        else
        {
            currentGrid = gridRun;
        }

        currentGrid.SetActive(true);
    }

    public void CurrentGridActive(bool isTrue)
    {
        currentGrid.SetActive(isTrue);
    }
}