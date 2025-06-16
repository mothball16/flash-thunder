using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Enums;
using FlashThunder.Gameplay.Events;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;

namespace FlashThunder.Core
{
    /// <summary>
    /// Bridges the input manager and the ECS world.
    /// This is initialized with GameContext and disposed with GameContext.
    /// </summary>
    public class InputBridge : IDisposable
    {
        private readonly World _world;
        private readonly InputManager<GameAction> _manager;

        public InputBridge(World world, InputManager<GameAction> manager)
        {
            _world = world;
            _manager = manager;
            Console.WriteLine(manager != null);
            manager.OnReleased += OnActionReleased;
            manager.OnActivated += OnActionActivated;
        }

        public void OnActionActivated(GameAction action)
        {
            _world.Publish<ActionActivatedEvent>(new(action));
        }

        public void OnActionReleased(GameAction action)
        {
            _world.Publish<ActionReleasedEvent>(new(action));
        }

        /// <summary>
        /// Disconnects the events.
        /// </summary>
        public void Dispose()
        {
            if (_manager == null) return;
            _manager.OnActivated -= OnActionActivated;
            _manager.OnReleased -= OnActionReleased;

            GC.SuppressFinalize(this);
        }
    }
}