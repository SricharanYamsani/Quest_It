using System;

[Serializable]
public class BattleBuffs
{
    public Attributes attackBuff = new Attributes();

    public Attributes defenseBuff = new Attributes();

    public Attributes agilityBuff = new Attributes();

    public Attributes luckBuff = new Attributes();

    public BattleBuffs()
    {
        attackBuff.current = 0;

        defenseBuff.current = 0;

        agilityBuff.current = 0;

        luckBuff.current = 0;

        agilityBuff.maximum = 100;

        attackBuff.maximum = 100;

        defenseBuff.maximum = 100;

        luckBuff.maximum = 30;
    }
}
