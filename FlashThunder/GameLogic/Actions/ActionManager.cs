using fennecs;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.GameLogic.Actions.Interfaces;
using FlashThunder.Utilities;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashThunder.GameLogic.Actions
{
    /// <summary>
    /// Holds attack behavior and handles attack lifetime.
    /// </summary>
    internal class ActionManager
    {
        private readonly Dictionary<string, AAttackBehavior> _attacks;
        private readonly List<ActionInstance> _instances;
        public ActionManager()
        {
            _attacks = [];
            _instances = [];
        }

        public ActionManager RegisterAttackBehavior(string name, AAttackBehavior behavior)
        {
            _attacks[name] = behavior;
            Logger.Print($"Registered {name} to the attack manager.");
            return this;
        }

        public ActionManager RegisterAttackBehavior(AAttackBehavior behavior)
            => RegisterAttackBehavior(behavior.GetType().Name, behavior);

        public void ExecuteAttack(World world, ActionData data)
        {
            if (!_attacks.TryGetValue(data.Behavior, out var behavior))
            {
                throw new KeyNotFoundException($"Attack behavior '{data.Behavior}' not found.");
            }
            _instances.Add(behavior.Execute(world, data));
        }

        public void Update(float dt)
        {
            List<ActionInstance> toRemove = [];
            foreach (var attack in _instances)
            {
                attack.Update(attack, dt);
                if (attack.IsOver)
                {
                    toRemove.Add(attack);
                }
            }
            foreach (var attack in toRemove)
                _instances.Remove(attack);
        }


    }
}