using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCharacteristicsManager : MonoBehaviour
{
    public Button closeButton;

    public PlayerInfo currentPlayerInfo;

    public List<PlayerStatUI> statsUI;

    public List<PlayerMoveUI> activeMoves;

    public List<PlayerMoveUI> otherMoves;

    private void Start()
    {
        SetUpButtons();

        currentPlayerInfo = GetCurrentPlayerInfo();

        if(currentPlayerInfo != null)
        {
            SetUpAllStatsUI(currentPlayerInfo);
            SetUpAllMovesUI(currentPlayerInfo);
        }

    }

    PlayerInfo GetCurrentPlayerInfo()
    {
        PlayerInfo player = null;
        return player;
    }

    void SetUpButtons()
    {
        closeButton.onClick.RemoveAllListeners();

        closeButton.onClick.AddListener(OnCloseButtonClicked);
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
