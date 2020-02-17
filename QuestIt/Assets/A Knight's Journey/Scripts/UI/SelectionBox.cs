using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SelectionBox : MonoBehaviour
{
    public RectTransform background;

    public List<TargetSelection> selectionBoxes = new List<TargetSelection>();

    private static Vector2 closed = new Vector2(0, 300);

    private static Vector2 opened = new Vector2(1600, 300);

    public CanvasGroup buttonGroup;

    public CanvasGroup backgroundGroup;

    public Image selectButtonImage;

    public Button selectButton;

    public Button backButton;

    public GameObject errorMessage;

    private bool allowSelection;

    private delegate void Callback();

    public void LoadOptions(List<BattlePlayer> battlePlayers, bool canSelect)
    {
        Reset();

        for (int i = 0; i < selectionBoxes.Count; i++)
        {
            TargetSelection selection = selectionBoxes[i];

            if (i < battlePlayers.Count)
            {
                selection.Setup(battlePlayers[i], canSelect);
            }
        }

        CheckForOptions(battlePlayers, canSelect);

        allowSelection = canSelect;

        OpenBox();
    }

    private void CheckForOptions(List<BattlePlayer> battlePlayers, bool canSelect)
    {
        if (battlePlayers.Count > 0)
        {
            errorMessage.SetActive(false);
        }
        else
        {
            errorMessage.SetActive(true);
        }

    }

    private void OpenBox()
    {
        SetSelectionButton();

        SetBackButton();

        SetBackgrooundOpen();

    }

    private void SetBackgrooundOpen()
    {
        background.gameObject.SetActive(true);

        background.DOKill(false);

        background.sizeDelta = closed;

        background.DOSizeDelta(opened, 0.4f).OnComplete(() =>
        {
            background.sizeDelta = opened;
        });
    }

    private void SetSelectionButton()
    {
        selectButton.onClick.AddListener(() => {

            OnClickSelectButton();
        });

        selectButtonImage.DOKill(false);

        selectButtonImage.gameObject.SetActive(true);

        selectButtonImage.fillAmount = 0;

        DOTween.To(() => selectButtonImage.fillAmount, x => selectButtonImage.fillAmount = x, 1, 0.4f);
    }

    private void SetBackButton()
    {
        backButton.onClick.AddListener(() =>
        {
            OnBackButton();

        });

        buttonGroup.DOKill(false);

        buttonGroup.alpha = 0;

        buttonGroup.gameObject.SetActive(true);

        buttonGroup.DOFade(1, 0.4f);
    }

    private void CloseBox(Callback callback = null)
    {
        selectButton.onClick.RemoveAllListeners();

        backButton.onClick.RemoveAllListeners();

        background.DOKill(false);

        buttonGroup.DOKill(false);

        backgroundGroup.DOKill(false);

        DOTween.To(() => selectButtonImage.fillAmount, x => selectButtonImage.fillAmount = x, 0, 0.4f).OnComplete(() =>
        {
            background.DOSizeDelta(closed, 0.4f).OnComplete(() =>
            {
                background.sizeDelta = closed;

                background.gameObject.SetActive(false);

            });

            buttonGroup.DOFade(0, 0.4f).OnComplete(() =>
            {
                buttonGroup.gameObject.SetActive(false);

            });

            selectButtonImage.gameObject.SetActive(false);

            selectButtonImage.fillAmount = 0;

            if (callback != null)
            {
                callback?.Invoke();
            }
        });
    }

    private void Reset()
    {
        foreach (TargetSelection box in selectionBoxes)
        {
            box.Setup(null);
        }
    }

    private void OnClickSelectButton()
    {
        bool flag = false;

        foreach (TargetSelection ts in selectionBoxes)
        {
            if (ts.isSelected)
            {
                flag = true;
            }
        }

        if (flag)
        {
            CloseBox(() =>
            {
                if (!allowSelection)
                {
                    foreach (TargetSelection selection in selectionBoxes)
                    {
                        if (selection.isSelected)
                        {
                            ChoiceManager.Instance.AddPlayer(selection.mPlayer, false);
                        }
                    }
                }
                ChoiceManager.Instance.OnSelectionCompleted();

            });
        }
    }

    private void OnBackButton()
    {
        BattleUIManager.Instance.CurrentGridActive(true);
         
        CloseBox();
    }
}
