using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCharacteristicsManager : MonoBehaviour
{
    public Button closeButton;

    public List<PlayerStatUI> statsUI;

    private void Start()
    {
        SetUpButtons();
    }

    void SetUpButtons()
    {
        closeButton.onClick.RemoveAllListeners();

        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    void SetUpStats()
    {
        //FIll stats using player qualities
    }

    void OnCloseButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

}
