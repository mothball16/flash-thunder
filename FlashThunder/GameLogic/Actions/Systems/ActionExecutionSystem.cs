using fennecs;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Movement.Components;

namespace FlashThunder.GameLogic.Actions.Systems
{
    internal sealed class ActionExecutionSystem : AUpdateSystem<float>
    {
        private readonly ActionLifetimeManager _manager;
        private readonly World _world;
        private readonly Stream<AttackQueued> _requestingAttack;
        public ActionExecutionSystem(World world, ActionLifetimeManager manager) : base()
        {
            _manager = manager;
            _world = world;
            _requestingAttack = world.Query<AttackQueued>()
                .Not<ExecutingActionTag>()
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
                    //TODO: This is bad design. The manager should be doing this instead
                    e.Add<ExecutingActionTag>();
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
