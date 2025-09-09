using fennecs;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.Utilities;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashThunder.GameLogic.Actions
{
    /// <summary>
    /// Holds action behavior and handles attack lifetime.
    /// </summary>
    internal class ActionLifetimeManager
    {
        private readonly Dictionary<string, AAttackBehavior> _attacks;
        private readonly List<ActionInstance> _instances;
        public ActionLifetimeManager()
        {
            _attacks = [];
            _instances = [];
        }

        public ActionLifetimeManager RegisterActionBehavior(string name, AAttackBehavior behavior)
        {
            _attacks[name] = behavior;
            Logger.Print($"Registered {name} to the attack manager.");
            return this;
        }

        public ActionLifetimeManager RegisterActionBehavior(AAttackBehavior behavior)
            => RegisterActionBehavior(behavior.GetType().Name, behavior);

        public void ExecuteAttack(World world, ActionData data)
        {
            if (!_attacks.TryGetValue(data.Behavior, out var behavior))
            {
                throw new KeyNotFoundException($"Attack behavior '{data.Behavior}' not found.");
            }
            var attackInstance = behavior.Execute(world, data);
            _instances.Add(attackInstance);
            attackInstance.OnStart?.Invoke(attackInstance);
        }

        public void Update(float dt)
        {
            List<ActionInstance> toRemove = [];
            foreach (var attack in _instances)
            {
                attack.Update?.Invoke(attack, dt);
                if (attack.IsOver)
                {
                    toRemove.Add(attack);
                }
            }
            foreach (var attack in toRemove)
            {
                attack.OnEnd?.Invoke(attack);
                _instances.Remove(attack);
            }
        }


    }
}