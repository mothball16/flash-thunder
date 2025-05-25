using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
using FlashThunder.ECS.Events;
using FlashThunder.Enums;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
namespace FlashThunder
{
    /// <summary>
    /// Bridges the input manager and the ECS world.
    /// This is initialized with GameContext and disposed with GameContext.
    /// </summary>
    public class InputBridge : IDisposable
    {
        private readonly World _world;
        public InputBridge(InputManager<PlayerAction> manager, World world) 
        {
            manager.OnReleased += OnActionReleased;
            manager.OnActivated += OnActionActivated;
        }
        
        public void OnActionActivated(PlayerAction action)
        {
            _world.Publish<ActionActivatedEvent>(new (action));
        }

        public void OnActionReleased(PlayerAction action)
        {
            _world.Publish<ActionReleasedEvent>(new(action));
        }

        public void Dispose()
        {

        }
    }
}
