using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        t_CurrentHealth = m_Player.CurrentHealth;

        t_ManaBar = m_Player.CurrentMana;

        manaBar.fillAmount = ((float)m_Player.CurrentMana) / (m_Player.MaxMana);

        healthBar.fillAmount = ((float)m_Player.CurrentHealth) / (m_Player.MaxHealth);

        this.m_Player = m_Player;

        //playerSprite.sprite = 
    }

    public void UpdateUI(PlayerUIUpdater updater)
    {
        switch (updater)
        {
            case PlayerUIUpdater.Health:
                if (t_CurrentHealth != m_Player.CurrentHealth)
                {
                    UpdateHealth(m_Player.CurrentHealth);
                }
                break;

            case PlayerUIUpdater.Mana:
                if (t_ManaBar != m_Player.CurrentMana)
                {
                    UpdateMana(m_Player.CurrentMana);
                }

                break;
            case PlayerUIUpdater.Both:

                if (t_CurrentHealth != m_Player.CurrentHealth)
                {
                    UpdateHealth(m_Player.CurrentHealth);
                }

                if (t_ManaBar != m_Player.CurrentMana)
                {
                    UpdateMana(m_Player.CurrentMana);
                }

                break;
        }
    }

    private void UpdateHealth(int changeInValue)
    {
        if (t_CurrentHealth <= 0 && changeInValue > 0)
        {
            m_Player.mPlayerController.SetTrigger(AnimationType.BACKTOLIFE.ToString());
        }

        DOTween.To(() => t_CurrentHealth, x => t_CurrentHealth = x, changeInValue, 0.4f).OnUpdate(() =>
        {
            healthBar.fillAmount = ((float)t_CurrentHealth / ((float)(m_Player.MaxHealth)));

        }).OnComplete(
            () =>
            {
                t_CurrentHealth = changeInValue;

                if(t_CurrentHealth<= 0)
                {
                    m_Player.mPlayerController.SetTrigger(AnimationType.DEAD.ToString());
                }
            }
            );
    }

    private void UpdateMana(int changeInValue)
    {
        DOTween.To(() => t_ManaBar, x => t_ManaBar = x, changeInValue, 0.4f).OnUpdate(() =>
        {

            manaBar.fillAmount = ((float)t_ManaBar / ((float)(m_Player.MaxMana)));

        }).OnComplete(
            () =>
            {
                t_ManaBar = changeInValue;
            }
            );
    }
}

public enum PlayerUIUpdater
{
    None,
    Mana,
    Health,
    Both
}
