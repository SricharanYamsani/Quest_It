using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRadialButton : MonoBehaviour
{
    public TypesOfChoices element;

    public void Toggle()
    {
        BattleUIManager.Instance.ShowChoices(true, element);
    }
}
