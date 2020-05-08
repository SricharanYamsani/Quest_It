using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUIConsumables : MonoBehaviour
{
    public BattleChoice m_Choice;

    public TextMeshProUGUI m_MoveName;

    public TextMeshProUGUI m_Description;

    public TextMeshProUGUI m_amountLeftText;

    public Image spriteIcon;

    private Button m_Button;

    private void Awake()
    {
        if (m_Button == null)
        {
            m_Button = GetComponent<Button>();
        }
    }

    public void SetupChoice(int amountLeft)
    {
        if (m_Choice)
        {
            if (m_MoveName)
            {
                m_MoveName.text = m_Choice.moveName;
            }
            if (m_Description)
            {
                m_Description.text = m_Choice.description;
            }
            if (m_amountLeftText)
            {
                m_amountLeftText.text = string.Format("Owned - {0}", amountLeft);
            }
            if(m_Choice.ICON)
            {
                spriteIcon.sprite = m_Choice.ICON;
            }

            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetCurrentChoice()
    {
        BattleManager.Instance.currentPlayer.currentChoice = m_Choice;

        BattleUIManager.Instance.ShowTargetChoices(m_Choice);
    }
}