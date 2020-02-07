using System.Collections;
using System.Collections.Generic;
using System;

public static class DamageCalculator
{
    public static int GetDamage(PlayerAttributes attacker, PlayerAttributes defender, int moveDamage)
    {
        int damage = (int)(Math.Ceiling((attacker.agility.current * 0.5f) / 10) + moveDamage);

        damage = (int)Math.Ceiling(damage - (defender.defense.current * 0.3f));

        return damage;
    }

    public static bool IsDodge(int defenderLuck)
    {
        Random random = new Random();

        int x = random.Next(0, 100);

        return (x <= defenderLuck);
    }
}