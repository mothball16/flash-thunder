using fennecs;
using FlashThunder.GameLogic.Attacks.Components;
using FlashThunder.GameLogic.Attacks.Data;
using FlashThunder.GameLogic.Attacks.Interfaces;
using FlashThunder.GameLogic.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Attacks.Behaviors
{
    internal class BasicAttackBehavior : IAttackBehavior
    {
        public void Execute(World world, AttackData data)
        {
            foreach(Entity opp in data.Opps)
            {
                if (data.Params is not DefaultAttackParams attackParams) return;

                var dmgVary = Random.Shared.Next(-attackParams.RandomRange, attackParams.RandomRange);
                var dmgFinal = attackParams.Damage + dmgVary;
                opp.Ref<TakeDamage>().Inflicts.Add(new(data.Attacker, dmgFinal));
            }
        }

    }
}
