using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    //<summary>Health Bar UI Reference</summary>
    public Image healthBar;

    //<summary>Mana Bar UI Reference</summary>
    public Image manaBar;

    //<summary>Player Sprite UI Reference</summary>
    public Image playerSprite;

    public BattlePlayer m_Player = null;

    private float t_CurrentHealth = 0.0f;

    private float t_ManaBar = 0.0f;

    public void Setup(BattlePlayer m_Player)
    {
        t_CurrentHealth = m_Player.attributes.health.current;

        healthBar.fillAmount = ((float)m_Player.attributes.health.current) / (m_Player.attributes.health.maximum);

        this.m_Player = m_Player;

        //playerSprite.sprite = 
    }

    public void UpdateUI()
    {
        StartCoroutine(UIUpdaterHealth());
    }

    private IEnumerator UIUpdaterHealth()
    {
        float difference = t_CurrentHealth - m_Player.attributes.health.current;

        int s_Frames = 60;

        float s_PerFrameDifference = difference / s_Frames;

        if (difference > 0)
        {
            for (int i = 0; i < 60; i++)
            {
                yield return null;

                t_CurrentHealth -= s_PerFrameDifference;

                t_CurrentHealth = Mathf.Clamp(t_CurrentHealth, 0, m_Player.attributes.health.maximum);

                healthBar.fillAmount = t_CurrentHealth / m_Player.attributes.health.maximum;
            }
        }
        else if (difference < 0)
        {
            for (int i = 0; i < 60; i++)
            {
                yield return null;

                t_CurrentHealth += s_PerFrameDifference;

                t_CurrentHealth = Mathf.Clamp(t_CurrentHealth, 0, m_Player.attributes.health.maximum);

                healthBar.fillAmount = t_CurrentHealth / m_Player.attributes.health.maximum;
            }
        }
        else
        {

        }

        t_CurrentHealth = m_Player.attributes.health.current;

        if (t_CurrentHealth <= 0)
        {
            m_Player.mPlayerController.SetTrigger(AnimationType.DEAD.ToString());
        }

        yield return null;
    }
}