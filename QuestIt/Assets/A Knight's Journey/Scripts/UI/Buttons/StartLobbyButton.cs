using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartLobbyButton : MonoBehaviour
{
    public GameObject lobbyGameObject;

    public List<Image> teamRed = new List<Image>();

    public List<Image> teamBlue = new List<Image>();

    private List<BattleCharacters> redCharacters = new List<BattleCharacters>();

    private List<BattleCharacters> blueCharacters = new List<BattleCharacters>();

    private BattleInitializer battleStance;

    public Image fillCircle;

    public TextMeshProUGUI fillAmount;

    private void Start()
    {
        battleStance = BattleInitializer.Instance;

        foreach (PlayerInfo pInfo in battleStance.lobbyPlayers)
        {
            if (pInfo.IsTeamRed)
            {
                redCharacters.Add(pInfo.character);
            }
            else
            {
                blueCharacters.Add(pInfo.character);
            }
        }

        for (int i = 0; i < teamRed.Count; i++)
        {
            if (i < redCharacters.Count)
            {
                teamRed[i].sprite = ResourceManager.Instance.playerIcons[redCharacters[i].ToString()];

                teamRed[i].gameObject.SetActive(true);
            }
            else
            {
                teamRed[i].gameObject.SetActive(false);
            }
        }
        for (int j = 0; j < teamBlue.Count; j++)
        {
            if (j < blueCharacters.Count)
            {
                teamBlue[j].sprite = ResourceManager.Instance.playerIcons[blueCharacters[j].ToString()];

                teamBlue[j].gameObject.SetActive(true);
            }
            else
            {
                teamBlue[j].gameObject.SetActive(false);
            }
        }

        fillCircle.fillAmount = 0;

        fillCircle.DOFillAmount(0, 5).OnUpdate(() =>
        {
            fillAmount.text = ((fillCircle.fillAmount) * 100).ToString("0");
        })
         .OnComplete(() =>
        {
            battleStance.StartLobby();

            lobbyGameObject.SetActive(false);
        });
    }
}
