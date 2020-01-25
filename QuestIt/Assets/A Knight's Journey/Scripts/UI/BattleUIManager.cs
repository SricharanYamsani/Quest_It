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

    //Selection
    public RectTransform targetSelectionScreen;
    public GameObject selectionBackground;

    public List<TargetSelection> selectors = new List<TargetSelection>();
    #endregion
    public TextMeshProUGUI mText;

    #region Constants
    public const float fadeTimeRadial = 0.4f;
    #endregion
    bool isSliderOn = false;


    private float sizeXSlider = 0.0f;

    [SerializeField]
    List<BaseUIChoice> t_Choices = new List<BaseUIChoice>();

    protected override void Awake()
    {
        base.Awake();

        CheckForSetup();

        gameOverScreen.SetActive(false);

        BattleManager.Instance.GameInit += LoadAllPlayerUI;

        BattleManager.Instance.GameOver += GameOverScreen;

        BattleManager.Instance.TurnStart += TurnStart;

        BattleManager.Instance.RoundOver += RoundOver;

        selectTrueButton.onClick.AddListener(() => { SelectPlayerGo(); });
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
        if (obj.IsPlayer)
        {
            ShowRadialButton(true);
        }
        else
        {
            if (RadialButton.gameObject.activeInHierarchy)
            {
                DOVirtual.DelayedCall(0.4f, () => { RadialButton.interactable = false; ShowRadialButton(false); });

                MoveOptionsLayer(false);

            }
        }
    }
    private void LoadAllPlayerUI()
    {
        foreach (BattlePlayer player in BattleManager.Instance.validPlayers)
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
            }
        }
    }

    public void ShowChoices(bool isTrue, RadialButtonElements buttonElement = RadialButtonElements.NONE)
    {
        if (isTrue)
        {
            switch (buttonElement)
            {
                case RadialButtonElements.ATTACK: SwitchGrids(0); break;
                case RadialButtonElements.DEFEND: SwitchGrids(1); break;
                case RadialButtonElements.ITEM: SwitchGrids(2); break;
                case RadialButtonElements.RUN: SwitchGrids(3); break;
            }
        }

        MoveOptionsLayer(isTrue);
    }

    #region TargetChoices
    public void ShowTargetChoices(AttackRange range)
    {
        List<BattlePlayer> myTargets = new List<BattlePlayer>();

        List<BattlePlayer> validPlayers = BattleManager.Instance.validPlayers;

        bool canSelect = false;

        switch (range)
        {
            case AttackRange.ONEENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (!battlePlayer.IsTeamRed)
                    {
                        myTargets.Add(battlePlayer);
                    }
                }
                canSelect = true;

                break;

            case AttackRange.ONETEAM:

                myTargets = BattleManager.Instance.currentBlue;

                canSelect = true;

                break;

            case AttackRange.ALLENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (!battlePlayer.IsTeamRed)
                    {
                        myTargets.Add(battlePlayer);
                    }
                }
                canSelect = false;

                break;

            case AttackRange.ALLTEAM:
                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (!battlePlayer.IsTeamRed)
                    {
                        myTargets.Add(battlePlayer);
                    }
                }
                canSelect = false;

                break;

            case AttackRange.EVERYONE:

                myTargets = validPlayers;

                canSelect = false;

                break;
        }

        if (myTargets.Count > 0)
        {
            LoadTargets(myTargets, canSelect);
        }
    }
    #endregion
    private void LoadTargets(List<BattlePlayer> targets, bool isSelection = false)
    {
        for (int i = 0; i < selectors.Count; i++)
        {
            if (i < targets.Count)
            {
                selectors[i].Setup(targets[i], isSelection);

                if (!isSelection)
                {
                    selectors[i].frame.gameObject.SetActive(true);
                }
            }
            else
            {
                selectors[i].Setup(null);
            }
        }

        // Selection Screen
        selectionBackground.SetActive(true);

        targetSelectionScreen.anchoredPosition = Vector2.zero;

        targetSelectionScreen.gameObject.SetActive(true);

        targetSelectionScreen.DOAnchorPosY(-250, 0.4f);
        // Selection Screen End
    }

    private void CancelSelection()
    {
        targetSelectionScreen.DOAnchorPosY(0, 0.4f).OnComplete(() => {

            targetSelectionScreen.gameObject.SetActive(false);

            selectionBackground.SetActive(false);

            targetSelectionScreen.anchoredPosition = Vector2.zero;

        });
    }

    public void CancelSelectionButton()
    {
        CancelSelection();
    }

    public void SelectPlayerGo()
    {
        List<BattlePlayer> targets = new List<BattlePlayer>();

        foreach (TargetSelection targetselector in selectors)
        {
            if (targetselector.isSelected)
            {
                targets.Add(targetselector.mPlayer);
            }
        }
        if (targets.Count > 0)
        {
            targetSelectionScreen.gameObject.SetActive(false);

            targetSelectionScreen.anchoredPosition = Vector2.zero;

            selectionBackground.SetActive(false);

            BattlePlayer myPlayer = BattleManager.Instance.currentPlayer;

            myPlayer.currentChoice.MoveWork(myPlayer, targets);

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

            selectionPanel.DOSizeDelta(new Vector2(1700, selectionPanel.sizeDelta.y), 0.4f).OnComplete(() => { selectionPanel.sizeDelta = new Vector2(1700, selectionPanel.sizeDelta.y); });
        }
        else
        {
            selectionPanel.sizeDelta = new Vector2(1700, selectionPanel.sizeDelta.y);

            selectionPanel.DOSizeDelta(new Vector2(0, selectionPanel.sizeDelta.y), 0.4f).OnComplete(() => { selectionPanel.gameObject.SetActive(false); selectionPanel.sizeDelta = new Vector2(0, selectionPanel.sizeDelta.y); });
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

    public void SwitchGrids(int index)
    {
        // Sricharan - To Fix
        gridAttack.SetActive(false);
        gridDefend.SetActive(false);
        gridItem.SetActive(false);
        gridRun.SetActive(false);

        if (index == 0)
        {
            sizeXSlider = BattleManager.Instance.currentPlayer.validChoices.Count * 300f;
            gridAttack.SetActive(true);
        }
        else if (index == 1)
        {
            gridDefend.SetActive(true);
        }
        else if (index == 2)
        {
            gridItem.SetActive(true);
        }
        else
        {
            gridRun.SetActive(true);
        }
    }
}