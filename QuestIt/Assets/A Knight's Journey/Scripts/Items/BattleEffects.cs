using System.Collections.Generic;

public class BattleEffects : IWeapon
{
    public override void Trigger(BattlePlayer targets = null)
    {
        base.Trigger(targets);

        targets.PlayReaction();
    }
}
