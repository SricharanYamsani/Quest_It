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

    public BattleBuffs battleBuffs = new BattleBuffs();

    public List<BattleChoice> validChoices = new List<BattleChoice>();

    public BattleChoice currentChoice = null;

    public PlayerIcon playerIcon;

    //[EnumNamedArray(typeof(EPlayerCamera))]
    //public List<RPG.CameraControl.VirtualCameraComponent> playerCameras;

    public Transform virtualCameraParent;
    public Transform playerVirtualCameraTransform;
    public float virtualCameraParentRotationY;

    public int UNIQUE_ID { get; set; }

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

    public Transform rightHand;

    public Transform leftHand;

    public Transform faceSpawn_head;

    public Transform faceSpawn_face;

    public Transform TorsoParent;

    #endregion

    public TextMeshProUGUI reactionText;

    #region Properties
    /// <summary>Returns true if Battle Player is the user. </summary>
    public bool IsPlayer { get { return playerInfo.IsPlayer; } }

    /// <summary>Returns true if Battle Player is Team Red. </summary
    public bool IsTeamRed { get { return playerInfo.IsTeamRed; } }

    public bool IsTacticDictationEnabled { get; set; } = false;

    public int CurrentAgility
    {
        get
        {
            return (playerInfo.myAttributes.agility.current + battleBuffs.agilityBuff.current);
        }
        set
        {
            battleBuffs.agilityBuff.current = value;

            battleBuffs.agilityBuff.current.Clamp(0, battleBuffs.agilityBuff.maximum);
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
            battleBuffs.defenseBuff.current = value;

            battleBuffs.defenseBuff.current.Clamp(0, battleBuffs.defenseBuff.maximum);
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

                BattleManager.Instance.InvokeUpdatePlayerList(); // If I was dead and was resurrected.
            }

            playerInfo.myAttributes.health.current = value;

            playerInfo.myAttributes.health.current.Clamp(0, playerInfo.myAttributes.health.maximum);

            if (!IsAlive)
            {
                BattleManager.Instance.InvokeUpdatePlayerList(); // If I die. Gets called Before UI Render.
            }
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

            playerInfo.myAttributes.mana.current.Clamp(0, playerInfo.myAttributes.mana.maximum);
        }
    }

    public int CurrentLuck
    {
        get
        {
            return playerInfo.myAttributes.luck.current + battleBuffs.luckBuff.current;
        }
        set
        {
            battleBuffs.luckBuff.current = value;

            battleBuffs.luckBuff.current.Clamp(0, battleBuffs.luckBuff.maximum);
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
            battleBuffs.attackBuff.current = value;

            battleBuffs.attackBuff.current.Clamp(0, battleBuffs.attackBuff.maximum);
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

    public PlayerState PlayerState { get; private set; } = PlayerState.NONE;

    private string currentTrigger = string.Empty;

    public void SetUpdateUI()
    {
        playerIcon.UpdateUI(PlayerUIUpdater.Both);
    }


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
        PlayerState = PlayerState.NONE;

        playerIcon.UpdateUI(PlayerUIUpdater.Both);
    }

    public void TurnStart(BattlePlayer player)
    {
        if (this == player)
        {
            if (this.IsAlive)
            {
                playerIcon.SetCurrent(true);

                if (this.IsPlayer || IsTacticDictationEnabled)
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
            PlayerState = PlayerState.HURT;
        }
        else
        {
            PlayerState = PlayerState.BLOCK;
        }
    }

    public void PlayReaction(string animation = "")
    {
        GameObject m = new GameObject();

        if (PlayerState == PlayerState.BLOCK)
        {
            mPlayerController.SetTrigger(AnimationType.BLOCK.ToString());

            reactionText.text = "MISS!";

            m = Instantiate<GameObject>(ResourceManager.Instance.reactionObjects["Shield"], leftHandSpawnOutside);
        }
        else if (PlayerState == PlayerState.HURT)
        {
            mPlayerController.SetTrigger(AnimationType.MIDHIT.ToString());

            m = Instantiate<GameObject>(ResourceManager.Instance.reactionObjects["Blood"], torsoTransform);

            reactionText.text = "HIT";
        }
        // need to show a shield or blood work.
        SetUpdateUI();

        DOVirtual.DelayedCall(3, () =>
        {
            Destroy(m);
        });

        PlayerState = PlayerState.NONE;
    }

    private void PerformMove(List<BattlePlayer> targets)
    {
        playerIcon.SetCurrent(false);

        currentChoice.MoveWork(this, targets);

        ChoiceManager.Instance.ExecutePlayerMove(this, currentChoice);

        BattleUIManager.Instance.CloseButton();
    }

    public void PlayAnimation(string animation = "")
    {
        if (!String.IsNullOrEmpty(animation))
        {
            if (!string.IsNullOrEmpty(currentTrigger))
            {
                mPlayerController.ResetTrigger(currentTrigger);
            }

            mPlayerController.SetTrigger(animation);

            currentTrigger = animation;
        }
    }
}