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
    public Button nextButton;
    public Button previousButton;

    int contentMovedCount = 0;

    public PlayerInfo currentPlayerInfo;

    public List<PlayerStatUI> statsUI;

    public PlayerMoveUI moveUIPrefab;
    public List<PlayerMoveUI> activeMoves;
    public List<PlayerMoveUI> otherMoves;

    public List<Moves> availableMoves;

    public Image playerIcon;

    public TextMeshProUGUI playerName;

    bool movesPanelActive;

    PlayerMoveUI currentChosenMove;
    PlayerMoveUI toChangeMove;

    [SerializeField]GridLayoutGroup contentGridLayout;

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
        previousButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();


        closeButton.onClick.AddListener(OnCloseButtonClicked);
        editButton.onClick.AddListener(OnEditButtonClicked);
        previousButton.onClick.AddListener(OnPreviousButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);

        nextButton.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(false);
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

    void OnNextButtonClicked()
    {
        otherMovesContent.GetComponent<RectTransform>().anchoredPosition += Vector2.up * 500f;

        contentMovedCount++;
        if (contentMovedCount == 2)
        {
            nextButton.gameObject.SetActive(false);
        }

        previousButton.gameObject.SetActive(true);
    }

    void OnPreviousButtonClicked()
    {
        otherMovesContent.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * 500f;

        contentMovedCount--;
        if (contentMovedCount == 0)
        {
            previousButton.gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(true);
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

        nextButton.gameObject.SetActive(enable);
        previousButton.gameObject.SetActive(enable);

        if (enable)
        {
            StartCoroutine(SetupOtherMovesUI());
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

    IEnumerator SetupOtherMovesUI()
    {
        previousButton.gameObject.SetActive(false);
        if(otherMovesContent.transform.childCount > 0)
        {
            yield break;
        }

        for (int i = 0; i < availableMoves.Count; i++)
        {
            PlayerMoveUI moveUI = Instantiate(moveUIPrefab, otherMovesContent.transform);
            moveUI.SetUpMoveUI(availableMoves[i]);
        }

        yield return null;

        contentGridLayout.enabled = false;
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
