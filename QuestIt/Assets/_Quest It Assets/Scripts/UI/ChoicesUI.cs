using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoicesUI : MonoBehaviour
{
    public BattleChoice battleChoiceOption;

    public TextMeshProUGUI mText;

    public void Start()
    {
        mText.text = battleChoiceOption.moveName;
    }

    public void ChooseOption()
    {
        BattleManager.Instance.currentPlayer.currentChoice = battleChoiceOption;

        BattleUIManager.Instance.ShowTargetChoices(battleChoiceOption.AttackValue);
    }
}
