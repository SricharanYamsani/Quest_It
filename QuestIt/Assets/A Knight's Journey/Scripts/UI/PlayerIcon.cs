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

        t_ManaBar = m_Player.attributes.mana.current;

        manaBar.fillAmount = ((float)m_Player.attributes.mana.current) / (m_Player.attributes.mana.maximum);

        healthBar.fillAmount = ((float)m_Player.CurrentHealth) / (m_Player.attributes.health.maximum);

        this.m_Player = m_Player;

        //playerSprite.sprite = 
    }

    public void UpdateUI(PlayerUIUpdater updater)
    {
        switch (updater)
        {
            case PlayerUIUpdater.Health:
                StartCoroutine(UIUpdaterHealth());
                break;

            case PlayerUIUpdater.Mana:

                StartCoroutine(UIUpadaterMana());

                break;
            case PlayerUIUpdater.Both:

                StartCoroutine(UIUpdaterHealth());

                StartCoroutine(UIUpadaterMana());

                break;
        }
    }

    private IEnumerator UIUpdaterHealth()
    {
        float difference = t_CurrentHealth - m_Player.CurrentHealth;

        if (difference != 0)
        {

            if (t_CurrentHealth == 0 && m_Player.CurrentHealth > 0)
            {
                m_Player.mPlayerController.SetTrigger(AnimationType.BACKTOLIFE.ToString()); // Resurrection
            }

            int s_Frames = 60;

            float s_PerFrameDifference = Mathf.Abs(difference / s_Frames);

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
                t_CurrentHealth = m_Player.attributes.health.current;
                yield return null;
            }

            t_CurrentHealth = m_Player.CurrentHealth;

            if (t_CurrentHealth <= 0)
            {
                m_Player.mPlayerController.SetTrigger(AnimationType.DEAD.ToString());

                SoundManager.Instance.PlaySound("Death");
            }
        }

        yield return null;
    }
    private IEnumerator UIUpadaterMana()
    {
        float difference = t_ManaBar - m_Player.attributes.mana.current;

        if (difference != 0)
        {

            int s_Frames = 60;

            float s_PerFrameDifference = Mathf.Abs(difference / s_Frames);

            if (difference > 0)
            {
                for (int i = 0; i < 60; i++)
                {
                    yield return null;

                    t_ManaBar -= s_PerFrameDifference;

                    t_ManaBar = Mathf.Clamp(t_ManaBar, 0, m_Player.attributes.mana.maximum);

                    manaBar.fillAmount = t_ManaBar / m_Player.attributes.mana.maximum;
                }
            }
            else if (difference < 0)
            {
                for (int i = 0; i < 60; i++)
                {
                    yield return null;

                    t_ManaBar += s_PerFrameDifference;

                    t_ManaBar = Mathf.Clamp(t_ManaBar, 0, m_Player.attributes.mana.maximum);

                    manaBar.fillAmount = t_ManaBar / m_Player.attributes.mana.maximum;
                }
            }
            else
            {

            }

            t_ManaBar = m_Player.attributes.mana.current;
        }
        yield return null;
    }
}

public enum PlayerUIUpdater
{
    None,
    Mana,
    Health,
    Both
}
