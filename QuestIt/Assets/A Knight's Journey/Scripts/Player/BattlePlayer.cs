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
    public PlayerQualities playerQualities = new PlayerQualities();

    public List<BattleChoice> validChoices = new List<BattleChoice>();

    public BattleChoice currentChoice = null;

    public PlayerIcon playerIcon;

    //[EnumNamedArray(typeof(EPlayerCamera))]
    //public List<RPG.CameraControl.VirtualCameraComponent> playerCameras;

    public Transform virtualCameraParent;
    public Transform playerVirtualCameraTransform;
    public float virtualCameraParentRotationY;

    public int UNIQUE_ID { get; private set; }

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

    public Transform LookAt;

    public Transform glowRing;

    #endregion

    public TextMeshProUGUI reactionText;

    #region Properties
    /// <summary>Returns true if Battle Player is the user </summary>
    public bool IsPlayer { get { return playerQualities.IsPlayer; } } 

    /// <summary>Returns true if Battle Player is Team Red </summary
    public bool IsTeamRed { get { return playerQualities.IsTeamRed; } }

    /// <summary>If player is defending or not. If it is. Changes Every Round </summary
    public bool IsDefending { get; set; } = false;

    public int CurrentAgility { get { return 0; } }

    public int CurrentDefence { get { return 0; } }

    public bool IsAlive { get { return playerQualities.myAttributes.health.current > 0; } }

    public int CurrentHealth
    {
        get
        {
            return playerQualities.myAttributes.health.current;
        }

        set
        {
            playerQualities.myAttributes.health.current = value;
        }
    }

    public int CurrentMana
    {
        get
        {
            return playerQualities.myAttributes.mana.current;
        }
        set
        {
            playerQualities.myAttributes.mana.current = value;
        }
    }

    public int CurrentLuck
    {
        get
        {
            return playerQualities.myAttributes.luck.current;
        }
        set
        {
            playerQualities.myAttributes.luck.current = value;
        }
    }

    public int CurrentAttack
    {
        get
        {
            return playerQualities.myAttributes.attack.current;
        }
        set
        {
            playerQualities.myAttributes.attack.current = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return playerQualities.myAttributes.health.maximum;
        }
    }

    public int MaxMana
    {
        get
        {
            return playerQualities.myAttributes.mana.maximum;
        }
    }
    #endregion

    public Animator mPlayerController;

    public Color playerColor = new Color(168, 168, 168, 255);

    public PlayerState m_PlayerState = PlayerState.NONE;

    public void SetPlayer()
    {
        validChoices.Clear();

        for (int i = 0; i < playerQualities.chosenMoves.Count; i++)
        {
            validChoices.Add(ResourceManager.Instance.GetChoiceFromMove[playerQualities.chosenMoves[i]]);
        }
        if (validChoices.Count > 0)
        {
            currentChoice = validChoices[0];
        }
    }

    public void ShowReaction(string animation = "")
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
                // change it to different Hits.
                mPlayerController.SetTrigger(AnimationType.MIDHIT.ToString());

                reactionText.text = "HIT";
            }
        }
        else
        {
            // Heal animation
        }
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
            if (this.IsAlive)
            {
                if (this.IsPlayer)
                {
                    BattleUIManager.Instance.ShowRadialButton(true);
                }
                else
                {
                    // AI Shit to go here.
                    AIChoice();
                }

                glowRing.gameObject.SetActive(true);
            }
            else
            {
                BattleManager.Instance.IsSelecting = false;
            }
        }
        else
        {
            if(glowRing.gameObject.activeInHierarchy)
            {
                glowRing.gameObject.SetActive(false);
            }
        }
    }

    private void AIChoice()
    {
        currentChoice = validChoices[UnityEngine.Random.Range(0, validChoices.Count)];

        List<BattlePlayer> myTargets = new List<BattlePlayer>();

        List<BattlePlayer> validPlayers = BattleManager.Instance.GetAllPlayers();

        bool canSelect = false;

        switch (currentChoice.AttackValue)
        {
            case AttackRange.ONEENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (this.IsTeamRed)
                    {
                        if (!battlePlayer.IsTeamRed && battlePlayer.IsAlive)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.IsTeamRed && battlePlayer.IsAlive)
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
                    myTargets = BattleManager.Instance.GetTeamBluePlayers();
                }
                else
                {
                    myTargets = BattleManager.Instance.GetTeamRedPlayers();
                }

                canSelect = true;

                break;

            case AttackRange.ALLENEMY:

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (this.IsTeamRed)
                    {
                        if (!battlePlayer.IsTeamRed && battlePlayer.IsAlive)
                        {
                            myTargets.Add(battlePlayer);
                        }
                    }
                    else
                    {
                        if (battlePlayer.IsTeamRed && battlePlayer.IsAlive)
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

                foreach (BattlePlayer battlePlayer in validPlayers)
                {
                    if (battlePlayer.IsAlive)
                    {
                        myTargets.Add(battlePlayer);
                    }
                }

                canSelect = false;

                break;
        }

        List<BattlePlayer> temp_Target = new List<BattlePlayer>();

        if (myTargets.Count > 0)
        {
            if (canSelect)
            {
                int x = UnityEngine.Random.Range(0, myTargets.Count);

                temp_Target.Add(myTargets[x]);
            }
            else
            {
                temp_Target = myTargets;
            }
            currentChoice.MoveWork(this, temp_Target);
        }
        else
        {
            BattleManager.Instance.IsSelecting = false;
        }
    }

    public void RoundOver()
    {

    }
}