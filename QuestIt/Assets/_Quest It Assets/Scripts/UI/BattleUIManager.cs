using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BattleUIManager : Singleton<BattleUIManager>
{
    #region ObjectReferences

    public RectTransform playerHUD;
    public RectTransform enemyHUD;

    public PlayerIcon playerIconRef;
    public PlayerIcon enemyIconRef;

    public CanvasGroup RadialButton;
    public GameObject gameOverScreen;

    public Button selectTrueButton;

    //Selection
    public RectTransform targetSelectionScreen;
    public List<TargetSelection> selectors = new List<TargetSelection>();
    #endregion
    public TextMeshProUGUI mText;

    #region Constants
    public const float fadeTimeRadial = 0.4f;
    #endregion
    bool isSliderOn = false;

    [SerializeField]
    List<BaseUIChoice> t_Choices = new List<BaseUIChoice>();

    protected override void Awake()
    {
        base.Awake();

        gameOverScreen.SetActive(false);

        BattleManager.Instance.GameInit += LoadAllPlayerUI;

        BattleManager.Instance.GameOver += GameOverScreen;

        selectTrueButton.onClick.AddListener(() => { SelectPlayerGo(); });
    }
    private void LoadAllPlayerUI()
    {
        foreach (BattlePlayer player in BattleManager.Instance.validPlayers)
        {
            RectTransform parentContent = null;

            PlayerIcon iconRef;

            if (player.isTeamRed)
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

            if (player.isPlayer)
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

    public void ShowRadial()
    {
        RadialButton.DOFade(1, fadeTimeRadial).OnKill(() => { RadialButton.alpha = 1; });
    }
    public void ShowChoices()
    {

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
                    if (!battlePlayer.isTeamRed)
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
                    if (!battlePlayer.isTeamRed)
                    {
                        myTargets.Add(battlePlayer);
                    }
                }
                canSelect = false;

                break;

            case AttackRange.ALLTEAM:
                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (!battlePlayer.isTeamRed)
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

        targetSelectionScreen.gameObject.SetActive(true);
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
        targetSelectionScreen.gameObject.SetActive(false);

        BattleManager.Instance.currentPlayer.currentChoice.MoveWork(targets);
    }

    private void FixedUpdate()
    {
        mText.text = BattleManager.Instance.m_Timer.ToString("00");
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}
