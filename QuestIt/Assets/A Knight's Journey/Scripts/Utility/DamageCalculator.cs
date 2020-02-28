using System.Collections;
using System.Collections.Generic;
using System;

public static class DamageCalculator
{
    public static int GetChangeUpValue(PlayerAttributes attacker, PlayerAttributes defender, BattleChoice move, ref bool isHurt)
    {
        int damage = 0;

        if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
        {
            if (!IsDodge(GetCurrentAttribute(defender, AttributeTypes.LUCK)))
            {
                damage = (int)(Math.Ceiling((GetCurrentAttribute(attacker, AttributeTypes.AGILITY) * 0.5f) / 10) +

                    (move.percentageType == PercentageType.NONE ? move.attributeChange : (percentageValues[move.percentageType] * GetMaxAttribute(attacker, move.affectedAttribute))));

                damage = (int)Math.Ceiling(damage - (GetCurrentAttribute(defender, AttributeTypes.DEFENSE) * 0.1f));

                damage.Clamp(0, GetMaxAttribute(defender, move.affectedAttribute));

                damage *= -1;

                isHurt = true;
            }
            else
            {
                isHurt = false;
            }
        }
        else if(move.battleTask == BattleTasks.ATTRIBUTE_ENHANCEMENT)
        {
            damage =(int)(move.percentageType == PercentageType.NONE ? move.attributeChange : percentageValues[move.percentageType] * GetMaxAttribute(attacker, move.affectedAttribute));

            damage.Clamp(0, GetMaxAttribute(defender, move.affectedAttribute));
        }

        return damage;
    }

    public static int GetCurrentAttribute(PlayerAttributes attributes, AttributeTypes types)
    {
        switch (types)
        {
            case AttributeTypes.ATTACK:

                return attributes.attack.current;

            case AttributeTypes.AGILITY:

                return attributes.agility.current;

            case AttributeTypes.DEFENSE:

                return attributes.defense.current;

            case AttributeTypes.LUCK:

                return attributes.luck.current;

            case AttributeTypes.HEALTH:

                return attributes.health.current;

            case AttributeTypes.MANA:

                return attributes.mana.current;

            default:

                Logger.Error("error in calculation");

                return -9999;
        }
    }

    public static int GetMaxAttribute(PlayerAttributes attributes, AttributeTypes types)
    {
        switch (types)
        {
            case AttributeTypes.ATTACK:

                return attributes.attack.maximum;

            case AttributeTypes.AGILITY:

                return attributes.agility.maximum;

            case AttributeTypes.DEFENSE:

                return attributes.defense.maximum;

            case AttributeTypes.LUCK:

                return attributes.luck.maximum;

            case AttributeTypes.HEALTH:

                return attributes.health.maximum;

            case AttributeTypes.MANA:

                return attributes.mana.maximum;
            default:

                Logger.Error("error in calculation");

                return -9999;
        }
    }

    public static Dictionary<PercentageType, float> percentageValues = new Dictionary<PercentageType, float>
    {
        {PercentageType.SMALL,0.25f },
        {PercentageType.MID,.5f },
        { PercentageType.FULL,1.0f}
    };

    public static float DamageMultiplier(Elemental attackingElement, Elemental defenderElement)
    {
        if (attackingElement == Elemental.FIRE && defenderElement == Elemental.EARTH)
        {
            return 2;
        }
        else if (attackingElement == Elemental.EARTH && defenderElement == Elemental.WIND)
        {
            return 2;
        }
        else if (attackingElement == Elemental.WIND && defenderElement == Elemental.WATER)
        {
            return 2;
        }
        else if (attackingElement == Elemental.WATER && defenderElement == Elemental.FIRE)
        {
            return 2;
        }
        else if (attackingElement == Elemental.EARTH && defenderElement == Elemental.WATER)
        {
            return 0.5f;
        }
        else if (attackingElement == Elemental.WIND && defenderElement == Elemental.EARTH)
        {
            return 0.5f;
        }
        else if (attackingElement == Elemental.WATER && defenderElement == Elemental.WIND)
        {
            return 0.5f;
        }
        else if (attackingElement == Elemental.FIRE && defenderElement == Elemental.WATER)
        {
            return 0.5f;
        }
        return 1;
    }

    public static bool IsDodge(int defenderLuck)
    {
        Random random = new Random();

        int x = random.Next(0, 100);

        return (x <= defenderLuck);
    }
}