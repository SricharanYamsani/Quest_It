using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;
using Photon.Realtime;

public class BattlePlayer : MonoBehaviour
{
    public PlayerAttributes attributes = new PlayerAttributes();

    public List<BattleChoice> validChoices = new List<BattleChoice>();

    public BattleChoice currentChoice = null;

    public List<BattlePlayer> targetEnemies = new List<BattlePlayer>();

    public List<BattlePlayer> teamPlayers = new List<BattlePlayer>();

    public List<BattlePlayer> target = new List<BattlePlayer>();

    public Transform WorldUI = null;

    public PlayerIcon playerIcon;

    public int UNIQUE_ID { get; private set; }

    public int CurrentAgility
    {
        get
        {
            return attributes.agility.current;
        }
    }

    [Space(20)]
    #region PositionReferences
    // Transforms for spawning Objects

    public Transform rightHandSpawnInside; // right hand inside

    public Transform rightHandSpawnOutside; // right hand outside - eg shield holder

    public Transform headSpawn; // head spawn for hat/helmet

    public Transform faceSpawn;// spawn for mask

    public Transform leftHandSpawnInside;

    public Transform leftHandSpawnOutside;

    public Transform meleeAttackSpawn;

    public Transform torsoTransform;

    public RectTransform reactionLabel;

    public Transform OriginalSpawn; // player spawn Position

    #endregion

    [Space(20)]
    [Header("UI OBJECTS")]
    public Image fillingHpBar;

    public TextMeshProUGUI reactionText;

    /// <summary>Returns true if Battle Player is the user </summary>
    public bool IsPlayer { get; private set; } = false;

    /// <summary>Returns true if Battle Player is Team Red </summary
    public bool IsTeamRed { get; private set; } = false;

    /// <summary>If player is defending or not. If it is. Changes Every Round </summary
    public bool isDefending = false;

    public Animator mPlayerController;

    private Sequence m_Sequence;

    private Color objectcolor = new Color(168, 168, 168, 255);

    public PlayerState m_PlayerState = PlayerState.NONE;

    private void Awake()
    {
        BattleManager.Instance.GameInit += SetArenaTargets;

        if (validChoices.Count > 0)
        {
            currentChoice = validChoices[0];
        }
    }

    public void SetPlayer(bool isPlayer, bool isTeamRed)
    {
        this.IsPlayer = isPlayer;
        this.IsTeamRed = isTeamRed;
    }


    private void SetArenaTargets()
    {
        if (IsTeamRed)
        {
            targetEnemies = BattleManager.Instance.currentRed;

            teamPlayers = BattleManager.Instance.currentBlue;
        }
        else
        {
            targetEnemies = BattleManager.Instance.currentBlue;

            teamPlayers = BattleManager.Instance.currentRed;
        }
    }

    public void SetTargets(BattlePlayer mPlayer)
    {
        if (!target.Contains(mPlayer))
        {
            target.Add(mPlayer);
        }
    }

    public void ShowReaction()
    {
        if (currentChoice.AttackStyle == ChoiceStyle.DEFEND)
        {
            mPlayerController.SetTrigger(AnimationType.BLOCK.ToString());
        }
        else if (currentChoice.AttackStyle == ChoiceStyle.ATTACK)
        {
            if (m_PlayerState == PlayerState.BLOCK)
            {
                mPlayerController.SetTrigger(AnimationType.BLOCK.ToString());

                reactionText.text = "MISS!";
            }
            else
            {
                mPlayerController.SetTrigger(AnimationType.HIT.ToString());

                reactionText.text = "HIT";
            }
        }
        else
        {
            // Heal animation
        }

        if (m_Sequence != null)
        {
            DOTween.Kill(m_Sequence);
        }

        m_Sequence = DOTween.Sequence();

        reactionLabel.gameObject.SetActive(true);

        m_Sequence.Append(reactionLabel.DOAnchorPos(new Vector2(0, 0.8f), 0.4f).SetEase(Ease.InQuad)).Join(DOVirtual.DelayedCall(0.1f, () => { reactionText.DOFade(0.1f, 0.3f); })).OnKill(() =>
{
    reactionLabel.anchoredPosition = Vector2.zero;

    reactionText.color = objectcolor;

    reactionLabel.gameObject.SetActive(false);

}).OnComplete(() =>
{

    reactionLabel.anchoredPosition = Vector2.zero;

    reactionText.color = objectcolor;

    reactionLabel.gameObject.SetActive(false);

});
    }

    public void TakePartInBattle(bool isTrue)
    {
        if (isTrue)
        {
            BattleManager.Instance.TurnStart += TurnStart;

            BattleManager.Instance.RoundStart += RoundStart;

            BattleManager.Instance.RoundOver += RoundOver;

            BattleManager.Instance.TurnOver += TurnOver;
        }
        else
        {
            BattleManager.Instance.TurnStart -= TurnStart;

            BattleManager.Instance.RoundStart -= RoundStart;

            BattleManager.Instance.RoundOver -= RoundOver;

            BattleManager.Instance.TurnOver -= TurnOver;
        }
    }

    private void RoundStart()
    {
        //this.attributes.mana.current += this.attributes.regenerationMana.current;

        //this.attributes.health.current += this.attributes.regenerationHealth.current;

        playerIcon.UpdateUI(PlayerUIUpdater.Both);
    }

    private void TurnOver()
    {
        playerIcon.UpdateUI(PlayerUIUpdater.Both);
    }

    public void TurnStart(BattlePlayer player)
    {
        if (this == player)
        {
            if (attributes.health.current > 0)
            {
                BattleManager.Instance.currentPlayer = this;

                if (this.IsPlayer)
                {
                    BattleUIManager.Instance.ShowRadialButton(true);
                }
                else
                {
                    // AI Shit to go here.
                    AIChoice();
                }
            }
            else
            {
                BattleManager.Instance.isSelecting = false;
            }
        }
        else
        {

        }
    }

    private void AIChoice()
    {
        currentChoice = validChoices[UnityEngine.Random.Range(0, validChoices.Count)];

        List<BattlePlayer> myTargets = new List<BattlePlayer>();

        List<BattlePlayer> validPlayers = BattleManager.Instance.validPlayers;

        bool canSelect = false;

        switch (currentChoice.AttackValue)
        {
            case AttackRange.ONEENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (this.IsTeamRed)
                    {
                        if (!battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                }
                canSelect = true;

                break;

            case AttackRange.ONETEAM:

                if (this.IsTeamRed)
                {
                    myTargets = BattleManager.Instance.currentBlue;
                }
                else
                {
                    myTargets = BattleManager.Instance.currentRed;
                }

                canSelect = true;

                break;

            case AttackRange.ALLENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (this.IsTeamRed)
                    {
                        if (!battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                }

                canSelect = false;

                break;

            case AttackRange.ALLTEAM:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (this.IsTeamRed)
                    {

                        if (!battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.IsTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                }

                canSelect = false;

                break;

            case AttackRange.EVERYONE:

                myTargets = validPlayers;

                canSelect = false;

                break;
        }

        List<BattlePlayer> temp_Target = new List<BattlePlayer>();

        if (canSelect)
        {
            temp_Target.Add(myTargets[0]);
        }
        else
        {
            temp_Target = myTargets;
        }
        currentChoice.MoveWork(this, temp_Target);
    }

    public void RoundOver()
    {

    }
}