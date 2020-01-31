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
                if (t_CurrentHealth != m_Player.attributes.health.current)
                {
                    UpdateHealth(m_Player.attributes.health.current);
                }
                break;

            case PlayerUIUpdater.Mana:
                if (t_ManaBar != m_Player.attributes.mana.current)
                {
                    UpdateMana(m_Player.attributes.mana.current);
                }

                break;
            case PlayerUIUpdater.Both:

                if (t_CurrentHealth != m_Player.attributes.health.current)
                {
                    UpdateHealth(m_Player.attributes.health.current);
                }

                if (t_ManaBar != m_Player.attributes.mana.current)
                {
                    UpdateMana(m_Player.attributes.mana.current);
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
            healthBar.fillAmount = ((float)t_CurrentHealth / ((float)(m_Player.attributes.health.maximum)));

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

            manaBar.fillAmount = ((float)t_ManaBar / ((float)(m_Player.attributes.mana.maximum)));

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
