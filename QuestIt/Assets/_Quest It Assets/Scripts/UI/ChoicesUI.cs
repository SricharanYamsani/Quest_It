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
        BattleUIManager.Instance.ourPlayer.currentChoice = battleChoiceOption;

        BattleUIManager.Instance.ToggleSlider ( );

    }
}
