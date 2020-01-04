using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : Singleton<PlayerGenerator>
{
    public PlayerAttributes AttributesGenerator()
    {
        PlayerAttributes m_attributes = new PlayerAttributes();

        m_attributes.maxHealth = 100;

        m_attributes.maxAgility = 90;

        m_attributes.maxDefense = 75;

        m_attributes.maxLuck = 30;

        m_attributes.curHealth = m_attributes.maxHealth;

        m_attributes.curAgility = Random.Range ( 5 , m_attributes.maxAgility );

        m_attributes.curDefense = Random.Range ( 5 , m_attributes.maxDefense );

        m_attributes.curLuck = Random.Range ( 0 , m_attributes.maxLuck );

        return m_attributes;
    }
}
