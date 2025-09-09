using fennecs;
using FlashThunder.GameLogic._Shared;
using FlashThunder.GameLogic._Shared.Services;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Actions.Data;
using System;

namespace FlashThunder.GameLogic.Actions.Behaviors;

internal class BasicAttackBehavior : AAttackBehavior
{
    private static void DealDamage(World world, ActionData data)
    {
        if (data.Params is not DefaultAttackParams attackParams) return;
       var victims = world.Get<LookupService>().EntitiesOnTile(data.Target);

        
        foreach (Entity opp in victims)
        {
            var dmgVary = Random.Shared.Next(-attackParams.RandomRange, attackParams.RandomRange);
            var dmgFinal = attackParams.Damage + dmgVary;
            opp.RefOrAdd<TakeDamage>().Inflicts.Add(new(data.Attacker, dmgFinal));
        }
        ReleaseAttackTag(data);
    }
    public override ActionInstance Execute(World world, ActionData data)
    {
        // this is a one-frame attack without a lifetime, so IsOver is immediately true
        return new ActionInstance()
        {
            Update = (_,_) => DealDamage(world, data),
            IsOver = true
        };
    }
}
