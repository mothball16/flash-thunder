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
    internal sealed class AttackExecutionSystem : AUpdateSystem<float>
    {
        private readonly AttackManager _manager;
        private readonly World _world;
        private readonly Stream<AttackQueued> _requestingAttack;
        public AttackExecutionSystem(World world, AttackManager manager) : base()
        {
            _manager = manager;
            _world = world;
            _requestingAttack = world.Query<AttackQueued>()
                .Not<ExecutingAttackTag>()
                .Stream();
        }

        public override void Update(float upd)
        {
            _requestingAttack.For((in Entity e, ref AttackQueued attackQueued) =>
            {
                if(attackQueued.Queue.Count > 0)
                {
                    var attack = attackQueued.Queue.Dequeue();
                    //(ExecutingAttackTag should be unassigned by the attack itself)
                    e.Add<ExecutingAttackTag>();
                    _manager.ExecuteAttack(_world, attack);
                } else
                {
                    e.Remove<AttackQueued>();
                }
            });

            // TODO: this should not be in AttackExecutionSystem. change this later
            _manager.Update(upd);
        }
    }
}
