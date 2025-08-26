using fennecs;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.GameLogic.Actions.Interfaces;
using System;

namespace FlashThunder.GameLogic.Actions.Behaviors;

internal class BasicAttackBehavior : AAttackBehavior
{
    private static void DealDamage(ActionData data)
    {
        if (data.Params is not DefaultAttackParams attackParams) return;
        /*
        foreach (Entity opp in data.Opps)
        {
            var dmgVary = Random.Shared.Next(-attackParams.RandomRange, attackParams.RandomRange);
            var dmgFinal = attackParams.Damage + dmgVary;
            opp.Ref<TakeDamage>().Inflicts.Add(new(data.Attacker, dmgFinal));
        }*/
        ReleaseAttackTag(data);
    }
    public override ActionInstance Execute(World world, ActionData data)
    {
        // this is a one-frame attack without a lifetime, so IsOver is immediately true
        return new ActionInstance()
        {
            Update = (_,_) => DealDamage(data),
            IsOver = true
        };
    }
}
