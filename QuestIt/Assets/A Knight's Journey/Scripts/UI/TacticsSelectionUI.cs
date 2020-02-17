using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TacticsSelectionUI : MonoBehaviour
{
    public TextMeshProUGUI characterText;

    public TextMeshProUGUI buttonText;

    public Button toggleTacticsButton;

    public BattlePlayer _player = null;

    public Image characterImage = null;

    public bool isPlayerControlled = false;

    public void Setup(BattlePlayer _player)
    {
        this._player = _player;

        isPlayerControlled = this._player.IsTacticDictationEnabled;

        toggleTacticsButton.onClick.AddListener(() => { OnButtonClickSelection(); });

        SetText();
    }

    private void OnButtonClickSelection()
    {
        isPlayerControlled = !isPlayerControlled;

        if(_player != null)
        {
            _player.IsTacticDictationEnabled = isPlayerControlled;

            Logger.Error("a");
        }

        SetText();
    }

    private void SetText()
    {
        buttonText.text = isPlayerControlled ? "Allow Free Choice" : "Dictacte Choice";
    }
}
