using fennecs;
using FlashThunder.GameLogic.Attacks.Components;
using FlashThunder.GameLogic.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Attacks.Systems
{
    internal sealed class TakeDamageProcessingSystem : AUpdateSystem<float>
    {
        private readonly Stream<TakeDamage, Health> _damageIsRequested;
        public TakeDamageProcessingSystem(World world) : base()
        {
            _damageIsRequested = world.Query<TakeDamage, Health>()
                .Not<DeadTag>()
                .Stream();
        }

        public override void Update(float upd)
        {
            _damageIsRequested.For((in Entity e, ref TakeDamage takeDamage, ref Health health) =>
            {
                foreach(var damageEntry in takeDamage.Inflicts)
                {
                    health.CurHealth = Math.Clamp(health.CurHealth - damageEntry.Amount, 0, health.MaxHealth);
                    if(health.CurHealth == 0)
                    {
                        e.Add<DeadTag>();
                        break;
                    }
                }
                e.Remove<TakeDamage>();
            });
        }
    }
}
