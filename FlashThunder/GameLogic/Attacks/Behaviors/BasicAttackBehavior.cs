using fennecs;
using FlashThunder.GameLogic.Attacks.Components;
using FlashThunder.GameLogic.Attacks.Data;
using FlashThunder.GameLogic.Attacks.Interfaces;
using System;

namespace FlashThunder.GameLogic.Attacks.Behaviors
{
    internal class BasicAttackBehavior : AAttackBehavior
    {
        private static void DealDamage(AttackData data)
        {
            if (data.Params is not DefaultAttackParams attackParams) return;

            foreach (Entity opp in data.Opps)
            {
                var dmgVary = Random.Shared.Next(-attackParams.RandomRange, attackParams.RandomRange);
                var dmgFinal = attackParams.Damage + dmgVary;
                opp.Ref<TakeDamage>().Inflicts.Add(new(data.Attacker, dmgFinal));
            }
            ReleaseAttackTag(data);
        }
        public override AttackInstance Execute(World world, AttackData data)
        {
            
            
            // this is a one-frame attack without a lifetime, so IsOver is immediately true
            return new AttackInstance()
            {
                Update = (_) => DealDamage(data),
                IsOver = true
            };
        }

    }
}
