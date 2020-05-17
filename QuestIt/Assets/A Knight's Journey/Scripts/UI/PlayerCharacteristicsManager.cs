using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerCharacteristicsManager : MonoBehaviour
{
    public GameObject statsContent;
    public GameObject otherMovesContent;

    public Button closeButton;
    public Button editButton;

    public PlayerInfo currentPlayerInfo;

    public List<PlayerStatUI> statsUI;

    public PlayerMoveUI moveUIPrefab;
    public List<PlayerMoveUI> activeMoves;
    public List<PlayerMoveUI> otherMoves;

    public List<Moves> availableMoves;

    public Image playerIcon;

    public Text playerName;

    bool movesPanelActive;

    PlayerMoveUI currentChosenMove;
    PlayerMoveUI toChangeMove;

    private void Start()
    {
        movesPanelActive = false;
        currentChosenMove = toChangeMove = null;

        SetUpButtons();
        currentPlayerInfo = GetCurrentPlayerInfo();

        if(currentPlayerInfo != null)
        {
            SetUpAllStatsUI(currentPlayerInfo);
            SetupOtherMoves(currentPlayerInfo);
            SetUpAllMovesUI(currentPlayerInfo);
            SetPlayerInfo();
        }
    }

    PlayerInfo GetCurrentPlayerInfo()
    {
        PlayerInfo player = null;
        player = UIManager.Instance.playerController.playerInfo;
        return player;
    }

    void SetUpButtons()
    {
        closeButton.onClick.RemoveAllListeners();
        editButton.onClick.RemoveAllListeners();

        closeButton.onClick.AddListener(OnCloseButtonClicked);
        editButton.onClick.AddListener(OnEditButtonClicked);
    }

    void SetUpAllStatsUI(PlayerInfo playerInfo)
    {
        PlayerAttributes attributes = playerInfo.myAttributes;

        statsUI[(int)EStat.Health].SetUpStat("HEALTH", attributes.health.maximum.ToString());
        statsUI[(int)EStat.Mana].SetUpStat("MANA", attributes.mana.maximum.ToString());
        statsUI[(int)EStat.Attack].SetUpStat("ATTACK", attributes.attack.maximum.ToString());
        statsUI[(int)EStat.Defense].SetUpStat("DEFENSE", attributes.defense.maximum.ToString());
        statsUI[(int)EStat.Luck].SetUpStat("LUCK", attributes.luck.maximum.ToString());
        statsUI[(int)EStat.Agility].SetUpStat("AGILITY", attributes.agility.maximum.ToString());
        statsUI[(int)EStat.RegenerationMana].SetUpStat("REG. MANA", attributes.regenerationMana.maximum.ToString());
        statsUI[(int)EStat.RegenerationHealth].SetUpStat("REG. HEALTH", attributes.regenerationHealth.maximum.ToString());
    }

    void SetUpAllMovesUI(PlayerInfo playerInfo)
    {
        List<Moves> chosenMoves = playerInfo.chosenMoves;

        for (int i = chosenMoves.Count - 1; i >= 0; i--)
        {
            activeMoves[i].SetUpMoveUI(chosenMoves[i]);
        }
    }

    void OnCloseButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

    void SetPlayerInfo()
    {
        playerIcon.sprite = UIManager.Instance.GetPlayerImage(currentPlayerInfo.character);
        playerName.text = currentPlayerInfo.character.ToString();
    }

    void OnEditButtonClicked()
    {
        movesPanelActive = !movesPanelActive;
        EnableOtherMovesPanel(movesPanelActive);
    }

    private void EnableOtherMovesPanel(bool enable)
    {
        otherMovesContent.SetActive(enable);
        statsContent.SetActive(!enable);

        if (enable)
        {
            SetupOtherMovesUI();
        }
    }

    void SetupOtherMoves(PlayerInfo playerInfo)
    {
        availableMoves = new List<Moves>();
        List<string> allMoves = (System.Enum.GetNames((typeof(Moves)))).ToList();

        for (int i = 0; i < allMoves.Count; i++)
        {
            if(allMoves[i].Contains("RESURRECT"))
            {
                continue;
            }

            Moves move = (Moves)(System.Enum.Parse(typeof(Moves), allMoves[i]));

            if (currentPlayerInfo.chosenMoves.Contains(move) || move == Moves.NONE)
            {
                continue;
            }

            availableMoves.Add(move);

        }
    }

    void SetupOtherMovesUI()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            PlayerMoveUI moveUI = Instantiate(moveUIPrefab, otherMovesContent.transform);
            moveUI.SetUpMoveUI(availableMoves[i]);
        }
    }

    public void OnMoveUIClicked(PlayerMoveUI moveUI)
    {
        if(!movesPanelActive)
        {
            return;
        }

        if(currentPlayerInfo.chosenMoves.Contains(moveUI.GetMove()))
        {
            currentChosenMove = moveUI;
            Debug.LogError("Chosen");
        }
        else
        {
            toChangeMove = moveUI;
            Debug.LogError("To change");
        }

        if(currentChosenMove != null && toChangeMove != null)
        {
            UpdateChosenMove();
        }
    }

    void UpdateChosenMove()
    {
        Moves current = currentChosenMove.GetMove();
        Moves toChange = toChangeMove.GetMove();

        currentPlayerInfo.chosenMoves.Remove(current);
        availableMoves.Remove(toChange);

        availableMoves.Add(current);
        currentPlayerInfo.chosenMoves.Add(toChange);

        toChangeMove.SetUpMoveUI(current);
        currentChosenMove.SetUpMoveUI(toChange);

        Debug.LogError("Changed");
        currentChosenMove = toChangeMove = null;
    }

    public enum EStat
    {
        None = -1,
        Health,
        Mana,
        Attack,
        Defense,
        Luck,
        Agility,
        RegenerationMana,
        RegenerationHealth
    }

}
