using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartLobbyButton : MonoBehaviour
{
    public GameObject lobbyGameObject;

    public Image teamRed;

    public Image teamBlue;

    private List<BattleCharacters> redCharacters = new List<BattleCharacters>();

    private List<BattleCharacters> blueCharacters = new List<BattleCharacters>();

    private BattleInitializer battleStance;

    public Image fillCircle;

    public TextMeshProUGUI fillAmount;

    public RectTransform redTransform;

    public RectTransform blueTransform;

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
        teamRed.sprite = ResourceManager.Instance.playerIcons[redCharacters[0].ToString()];

        teamBlue.sprite = ResourceManager.Instance.playerIcons[blueCharacters[0].ToString()];

        redTransform.anchoredPosition = blueTransform.anchoredPosition = new Vector2(0, -1030);

        Sequence m_Sequence = DOTween.Sequence();

        fillCircle.fillAmount = 0;

        m_Sequence.Append(redTransform.DOAnchorPosY(0, 0.7f));

        m_Sequence.Join(blueTransform.DOAnchorPosY(0, 0.75f));

        m_Sequence.Append(fillCircle.DOFillAmount(1, 5).OnUpdate(() =>
        {
            fillAmount.text = ((fillCircle.fillAmount) * 100).ToString("0");
        }));

        m_Sequence.OnComplete(() =>
        {
            battleStance.StartLobby();

            lobbyGameObject.SetActive(false);
        });
    }
}
