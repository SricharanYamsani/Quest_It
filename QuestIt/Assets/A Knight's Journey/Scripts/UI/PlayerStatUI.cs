using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public Image statIcon;
    public TextMeshProUGUI descriptionText;

    public void SetUpStat(string descText, string valText, Sprite sprite = null)
    {
        valueText.text = valText;
        descriptionText.text = descText;

        if (sprite != null)
        {
            statIcon.sprite = sprite;
        }
    }
}
