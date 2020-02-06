using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    private static TargetSelection currentSelection;

    private void Awake()
    {
        selectionButton.onClick.AddListener(() =>
        {
            OnSelect();
        });

        frame.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ChoiceManager.Instance.OnAddingSelection += Listen;
    }
    private void OnDisable()
    {
        ChoiceManager.Instance.OnAddingSelection -= Listen;
    }

    private void Listen(BattlePlayer player)
    {
        if(player == mPlayer)
        {
            isSelected = true;

            selectionButton.interactable = false;

            frame.gameObject.SetActive(true);
        }
        else
        {
            ChoiceManager.Instance.InvokeRemoveMyPlayer(mPlayer);

            isSelected = false;

            selectionButton.interactable = true;

            frame.gameObject.SetActive(false);
            // this is not me. I should de select if I am selected.
        }
    }

    public void OnSelect()
    {
        ChoiceManager.Instance.AddPlayer(mPlayer);
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