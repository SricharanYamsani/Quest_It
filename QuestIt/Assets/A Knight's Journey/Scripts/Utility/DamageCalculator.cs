﻿using System.Collections;
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
                damage = (int)(Math.Ceiling((GetCurrentAttribute(attacker, AttributeTypes.AGILITY) * 0.5f) / 10) + move.attributeChange);

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
            damage = move.attributeChange;

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
        {PercentageType.SMALL,25.0f },
        {PercentageType.MID,50.0f },
        { PercentageType.FULL,100.0f}
    };

    public static bool IsDodge(int defenderLuck)
    {
        Random random = new Random();

        int x = random.Next(0, 100);

        return (x <= defenderLuck);
    }


    // Extension Funcions. Move to Utilities soon.
    public static int RoundToInt(this float x)
    {
        float decimalValue = x - (int)x;

        if (decimalValue > 0.5f)
        {
            return (int)x + 1;
        }

        return (int)x;
    }

    public static int Clamp(this int x, int min, int max)
    {
        if (x < min)
        {
            x = min;
        }
        else if (x > max)
        {
            x = max;
        }

        return x;
    }
    public static float Clamp(this float x, float min, float max)
    {
        if (x < min)
        {
            x = min;
        }
        else if (x > max)
        {
            x = max;
        }

        return x;
    }
}