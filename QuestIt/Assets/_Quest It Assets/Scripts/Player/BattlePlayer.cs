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

    private int uniqueID;

    public int UNIQUE_ID
    {
        get
        {
            return uniqueID;
        }
    }

    private int fakeCurrentHealth;

    public int turnIndex = 0;

    public int additionalAgility = 0;

    public int currentAgility
    {
        get
        {
            return additionalAgility + attributes.curAgility;
        }
    }

    // Transforms for spawning Objects

    public Transform rightHandSpawnInside;

    public Transform rightHandSpawnOutside;

    public Transform headSpawn;

    public Transform faceSpawn;

    public Transform leftHandSpawnInside;

    public Transform leftHandSpawnOutside;

    public Transform meleeAttackSpawn;

    public Transform torsoTransform;

    public RectTransform reactionLabel;

    public Transform OriginalSpawn;

    [Header("UI OBJECTS")]
    public Image fillingHpBar;

    public TextMeshProUGUI reactionText;

    public bool isPlayer = false;

    public bool isTeamRed = false;

    public Animator mPlayerController;

    private Coroutine mCoroutine;

    private Sequence m_Sequence;

    private delegate void onComplete();

    onComplete coroutineComplete = null;

    private Color objectcolor = new Color(168, 168, 168, 255);

    public PlayerState m_PlayerState = PlayerState.NONE;

    private void Awake()
    {
        BattleManager.Instance.GameInit += SetArenaTargets;

        uniqueID = BattleManager.uniqueID++;

        fakeCurrentHealth = attributes.curHealth;

        if (validChoices.Count > 0)
        {
            currentChoice = validChoices[0];
        }

        fakeCurrentHealth = attributes.curHealth;
    }

    private void SetArenaTargets()
    {
        if (isTeamRed)
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

    public void UpdateHealth()
    {
        if (playerIcon)
        {

            Debug.Log(this.name);

            DOTween.To(() => fakeCurrentHealth, x => fakeCurrentHealth = x, attributes.curHealth, 1).OnUpdate(() =>
   {
       if (WorldUI)
       {
           playerIcon.healthBar.fillAmount = (float)fakeCurrentHealth / attributes.maxHealth;
       }

   }).OnComplete(() =>
         {

             if (WorldUI)
             {
                 playerIcon.healthBar.fillAmount = (float)fakeCurrentHealth / attributes.maxHealth;

                 if (attributes.curHealth <= 0)
                 {
                     mPlayerController.SetTrigger(AnimationType.DEAD.ToString());
                 }
             }
         });
        }
    }

    public void TakePartInBattle(bool isTrue)
    {
        if (isTrue)
        {
            BattleManager.Instance.TurnStart += TurnStart;

            BattleManager.Instance.RoundOver += RoundOver;

            BattleManager.Instance.TurnOver += TurnOver;
        }
        else
        {
            BattleManager.Instance.TurnStart -= TurnStart;

            BattleManager.Instance.RoundOver -= RoundOver;

            BattleManager.Instance.TurnOver -= TurnOver;
        }
    }

    private void TurnOver()
    {
        playerIcon.UpdateUI();
    }

    public void TurnStart(BattlePlayer player)
    {
        if (this == player)
        {
            if (attributes.curHealth > 0)
            {
                BattleManager.Instance.currentPlayer = this;

                if (this.isPlayer)
                {
                    BattleUIManager.Instance.ShowChoices();
                    Debug.LogWarning("Player");
                }
                else
                {
                    // AI Shit to go here.
                    AIChoice();
                    Debug.LogWarning("Enemy");
                }
            }
            else
            {
                BattleManager.Instance.isSelecting = false;

                Debug.LogWarning("Cannot take part");
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
                    if (this.isTeamRed)
                    {
                        if (!battlePlayer.isTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.isTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                }
                canSelect = true;

                break;

            case AttackRange.ONETEAM:

                if (this.isTeamRed)
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
                    if (this.isTeamRed)
                    {
                        if (!battlePlayer.isTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.isTeamRed)
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
                    if (this.isTeamRed)
                    {

                        if (!battlePlayer.isTeamRed)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.isTeamRed)
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
        currentChoice.MoveWork(temp_Target);
    }

    public void RoundOver()
    {

    }
}