using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveUI : MonoBehaviour
{
    public Image image;
    Moves move;
    [SerializeField] Button button;

    public void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnMoveUIClicked);
    }

    public void SetUpMoveUI(Moves _move)
    {
        move = _move;

        try
        {
            MoveChoice choice = ResourceManager.Instance.GetChoiceFromMove[_move];
            if(choice.ICON != null)
            {
                image.sprite = choice.ICON;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(move.ToString() + " " + e);
        }
    }

    public Moves GetMove()
    {
        return move;
    }

    private void OnMoveUIClicked()
    {
        Debug.LogError("OMD");
        if(UIManager.Instance.playerCharacteristicsManager != null)
        {
            UIManager.Instance.playerCharacteristicsManager.OnMoveUIClicked(this);
        }
    }
}
