using System.Collections;
using System.Collections.Generic;
using System;

public static class DamageCalculator
{
    public static int GetDamage(PlayerAttributes attacker, PlayerAttributes defender, BattleChoice move, ref bool isHurt)
    {
        int damage = 0;

        if (move.affectedAttribute == AttributeTypes.HEALTH)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {
                if (!IsDodge(defender.luck.current))
                {
                    damage = (int)(Math.Ceiling((attacker.agility.current * 0.5f) / 10) + move.attributeChange);

                    damage = (int)Math.Ceiling(damage - (defender.defense.current * 0.1f));

                    damage.Clamp(0, defender.health.maximum);

                    damage *= -1;

                    isHurt = true;
                }
                else
                {
                    isHurt = false;
                }
            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.health.maximum);
            }
        }
        else if (move.affectedAttribute == AttributeTypes.MANA)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {

            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.mana.maximum);
            }
        }
        else if (move.affectedAttribute == AttributeTypes.AGILITY)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {

            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.agility.maximum);
            }
        }
        else if (move.affectedAttribute == AttributeTypes.ATTACK)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {

            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.attack.maximum);
            }
        }
        else if (move.affectedAttribute == AttributeTypes.DEFENSE)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {

            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.defense.maximum);
            }
        }
        else if (move.affectedAttribute == AttributeTypes.LUCK)
        {
            if (move.battleTask == BattleTasks.ATTRIBUTE_DECREMENT)
            {

            }
            else
            {
                damage = move.attributeChange;

                damage.Clamp(0, defender.luck.maximum);
            }
        }
        else
        {
            Logger.Error("Logical Error");
        }

        return damage;
    }

    public static bool IsDodge(int defenderLuck)
    {
        Random random = new Random();

        int x = random.Next(0, 100);

        return (x <= defenderLuck);
    }

    public static int Clamp(this int x, int min, int max)
    {
        if (x < min)
        {
            x = 0;
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
            x = 0;
        }
        else if (x > max)
        {
            x = max;
        }

        return x;
    }
}