using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerGenerator
{
    public static PlayerAttributes AttributesGenerator()
    {
        PlayerAttributes m_attributes = new PlayerAttributes();

        m_attributes.health.maximum = 100;

        m_attributes.agility.maximum = 90;

        m_attributes.defense.maximum = 75;

        m_attributes.luck.maximum = 30;

        m_attributes.attack.maximum = 100;

        m_attributes.mana.maximum = 200;

        m_attributes.regenerationHealth.maximum = 50;

        m_attributes.regenerationMana.maximum = 20;

        m_attributes.health.current = m_attributes.health.maximum;

        m_attributes.agility.current = Random.Range(5, m_attributes.agility.maximum);

        m_attributes.defense.current = Random.Range(5, m_attributes.defense.maximum);

        m_attributes.luck.current = Random.Range(0, m_attributes.luck.maximum);

        m_attributes.attack.current = Random.Range(5, m_attributes.attack.maximum);

        m_attributes.mana.current = Random.Range(80, m_attributes.mana.maximum);

        m_attributes.regenerationMana.current = Random.Range(5, m_attributes.regenerationMana.maximum);

        m_attributes.regenerationHealth.current = Random.Range(5, m_attributes.regenerationHealth.maximum);

        return m_attributes;
    }
}
