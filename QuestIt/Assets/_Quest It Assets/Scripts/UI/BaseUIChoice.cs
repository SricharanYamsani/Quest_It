using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUIChoice : MonoBehaviour
{
    public BattleChoice m_Choice;

    public Image m_Icon = null;

    public Image choiceStyleIcon;

    public Image currencyIcon;

    public TextMeshProUGUI m_MoveName;

    public TextMeshProUGUI m_Description;

    public TextMeshProUGUI m_CurrencyText;

    public TextMeshProUGUI m_ChoiceStyle;

    private Button m_Button;

    private void Awake ( )
    {
        m_Button = GetComponent<Button> ( );   
    }
    public void SetupChoice ( )
    {
        if ( m_Choice )
        {
            if ( m_Icon )
            {
                m_Icon.sprite = m_Choice.ICON;
            }

            if ( m_MoveName )
            {
                m_MoveName.text = m_Choice.moveName;
            }

            if ( m_Description )
            {
                m_Description.text = m_Choice.description;
            }

            if ( choiceStyleIcon )
            {
                choiceStyleIcon.sprite = ResourceManager.Instance.choiceSpritesRef [ m_Choice.AttackStyle.ToString ( ) ];
            }
            if ( currencyIcon )
            {
                currencyIcon.sprite = ResourceManager.Instance.currencySpritesRef [ m_Choice.m_Currency.ToString ( ) ];
            }
            if(m_ChoiceStyle)
            {
                m_ChoiceStyle.text = m_Choice.healthChange.ToString ( );
            }
            if(m_CurrencyText)
            {
                m_CurrencyText.text = m_Choice.m_CurrencyAmount.ToString ( );
            }
        }
    }

    public void SetCurrentChoice()
    {
        BattleUIManager.Instance.ourPlayer.currentChoice = m_Choice;

        BattleManager.Instance.mTimer = 0;
    }
}
