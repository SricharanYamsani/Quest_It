using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveUI : MonoBehaviour
{
    public Image image;
    Moves move;
    
    public void SetUpMoveUI(Moves _move)
    {
        move = _move;

        MoveChoice choice = ResourceManager.Instance.GetChoiceFromMove[_move];
        if(choice.ICON != null)
        {
            image.sprite = choice.ICON;
        }
    }
}
