using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// For Choosing a Single Player in Team
public class TargetSelection : MonoBehaviour
{
    #region UI REFERENCES
    [Header("REFERENCES")]
    public Image characterSprite;

    public Image frame;

    public Button selectionButton;
    #endregion

    [Header("VARIABLES")]
    public BattlePlayer mPlayer;

    public bool isSelected;

    private void Awake()
    {
        selectionButton.onClick.AddListener(() =>
        {
            OnSelect(this);
        });

        frame.gameObject.SetActive(false);
    }

    public void OnSelect(TargetSelection selectionBox)
    {
        if (selectionBox == this)
        {
            isSelected = this;

            selectionButton.interactable = false;
        }
        else
        {
            isSelected = false;

            selectionButton.interactable = true;
        }

        frame.gameObject.SetActive(isSelected);
    }

    public void Setup(BattlePlayer player, bool isInteractable = true)
    {
        if (player != null)
        {
            mPlayer = player;

            selectionButton.interactable = isInteractable;

            this.gameObject.SetActive(true);

            if (!isInteractable)
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            frame.gameObject.SetActive(isSelected);
            //characterSprite = ResourceManager.Instance.
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}