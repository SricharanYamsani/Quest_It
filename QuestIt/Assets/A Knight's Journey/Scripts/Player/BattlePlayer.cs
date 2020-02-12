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
    public PlayerInfo playerInfo = new PlayerInfo();

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

    public Transform bottomTransform;

    public Transform LookAt;

    public Transform glowRing;

    #endregion

    public TextMeshProUGUI reactionText;

    #region Properties
    /// <summary>Returns true if Battle Player is the user </summary>
    public bool IsPlayer { get { return playerInfo.IsPlayer; } }

    /// <summary>Returns true if Battle Player is Team Red </summary
    public bool IsTeamRed { get { return playerInfo.IsTeamRed; } }

    /// <summary>If player is defending or not. If it is. Changes Every Round </summary
    public bool IsDefending { get; set; } = false;

    public int CurrentAgility
    {
        get
        {
            return playerInfo.myAttributes.agility.current;
        }
        set
        {
            playerInfo.myAttributes.agility.current = value;
        }
    }

    public int CurrentDefense
    {
        get
        {
            return playerInfo.myAttributes.defense.current;
        }
        set
        {
            playerInfo.myAttributes.defense.current = value;
        }
    }

    public bool IsAlive { get { return CurrentHealth > 0; } }

    public int CurrentHealth
    {
        get
        {
            return playerInfo.myAttributes.health.current;
        }

        set
        {
            if (!IsAlive && value > 0)
            {
                mPlayerController.SetTrigger(AnimationType.BACKTOLIFE.ToString());
            }

            playerInfo.myAttributes.health.current = value;
        }
    }

    public int CurrentMana
    {
        get
        {
            return playerInfo.myAttributes.mana.current;
        }
        set
        {
            playerInfo.myAttributes.mana.current = value;
        }
    }

    public int CurrentLuck
    {
        get
        {
            return playerInfo.myAttributes.luck.current;
        }
        set
        {
            playerInfo.myAttributes.luck.current = value;
        }
    }

    public int CurrentAttack
    {
        get
        {
            return playerInfo.myAttributes.attack.current;
        }
        set
        {
            playerInfo.myAttributes.attack.current = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return playerInfo.myAttributes.health.maximum;
        }
    }

    public int MaxMana
    {
        get
        {
            return playerInfo.myAttributes.mana.maximum;
        }
    }
    public int MaxAgility
    {
        get
        {
            return playerInfo.myAttributes.agility.maximum;
        }
    }
    public int MaxLuck
    {
        get
        {
            return playerInfo.myAttributes.luck.maximum;
        }
    }
    public int MaxDefense
    {
        get
        {
            return playerInfo.myAttributes.defense.maximum;
        }
    }
    public int MaxAttack
    {
        get
        {
            return playerInfo.myAttributes.attack.maximum;
        }
    }
    #endregion

    public Animator mPlayerController;

    public Color playerColor = new Color(168, 168, 168, 255);

    public PlayerState m_PlayerState { get; private set; } = PlayerState.NONE;

    public void SetPlayer()
    {
        validChoices.Clear();

        for (int i = 0; i < playerInfo.chosenMoves.Count; i++)
        {
            validChoices.Add(ResourceManager.Instance.GetChoiceFromMove[playerInfo.chosenMoves[i]]);
        }
        if (validChoices.Count > 0)
        {
            currentChoice = validChoices[0];
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
        m_PlayerState = PlayerState.NONE;

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
            if (glowRing.gameObject.activeInHierarchy)
            {
                glowRing.gameObject.SetActive(false);
            }
        }
    }

    private void AIChoice()
    {
        currentChoice = validChoices[UnityEngine.Random.Range(0, validChoices.Count)];

        bool canSelect = false;

        List<BattlePlayer> myTargets = BattleManager.Instance.GetTargetPlayers(currentChoice, ref canSelect);

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
            foreach (BattlePlayer target in temp_Target)
            {
                ChoiceManager.Instance.AddPlayer(target, false);
            }

            ChoiceManager.Instance.OnSelectionCompleted();
        }
        else
        {
            BattleManager.Instance.IsSelecting = false;
        }
    }

    public void RoundOver()
    {

    }

    public void PerformMoveFocus(bool focus)
    {
        if (focus)
        {
            ChoiceManager.Instance.OnChoiceSelectionCompleted += PerformMove;
        }
        else
        {
            ChoiceManager.Instance.OnChoiceSelectionCompleted -= PerformMove;
        }
    }

    public void SetReaction(bool isHurt)
    {
        if (isHurt)
        {
            m_PlayerState = PlayerState.HURT;
        }
        else
        {
            m_PlayerState = PlayerState.BLOCK;
        }

        Logger.Error(isHurt.ToString());
    }

    public void PlayReaction(string animation = "")
    {
        if (m_PlayerState == PlayerState.BLOCK)
        {
            mPlayerController.SetTrigger(AnimationType.BLOCK.ToString());

            reactionText.text = "MISS!";
        }
        else if (m_PlayerState == PlayerState.HURT)
        {
            // change it to different Hits.
            mPlayerController.SetTrigger(AnimationType.MIDHIT.ToString());

            reactionText.text = "HIT";
        }

        m_PlayerState = PlayerState.NONE;
    }

    private void PerformMove(List<BattlePlayer> targets)
    {
        currentChoice.MoveWork(this, targets);

        ChoiceManager.Instance.ExecutePlayerMove(this, currentChoice);
    }
}