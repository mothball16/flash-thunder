using fennecs;
using FlashThunder.GameLogic.Attacks.Data;
using FlashThunder.GameLogic.Attacks.Interfaces;
using FlashThunder.Utilities;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Attacks
{
    /// <summary>
    /// Holds attack behavior and handles attack lifetime.
    /// </summary>
    internal class AttackManager
    {
        private readonly Dictionary<string, AAttackBehavior> _attacks;
        private readonly List<AttackInstance> _instances;
        public AttackManager()
        {
            _attacks = [];
            _instances = [];
        }

        public AttackManager RegisterAttackBehavior(string name, AAttackBehavior behavior)
        {
            _attacks[name] = behavior;
            Logger.Print($"Registered {name} to the attack manager.");
            return this;
        }

        public AttackManager RegisterAttackBehavior(AAttackBehavior behavior)
            => RegisterAttackBehavior(behavior.GetType().Name, behavior);

        public void ExecuteAttack(World world, AttackData data)
        {
            if (!_attacks.TryGetValue(data.Behavior, out var behavior))
            {
                throw new KeyNotFoundException($"Attack behavior '{data.Behavior}' not found.");
            }
            _instances.Add(behavior.Execute(world, data));
        }

        public void Update(float dt)
        {
            List<AttackInstance> toRemove = [];
            foreach (var attack in _instances)
            {
                attack.Update(dt);
                if (attack.IsOver)
                    toRemove.Add(attack);
            }
            foreach (var attack in toRemove)
                _instances.Remove(attack);
        }


    }
}