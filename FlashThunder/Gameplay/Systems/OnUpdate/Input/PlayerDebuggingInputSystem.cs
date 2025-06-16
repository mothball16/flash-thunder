using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Enums;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Events;
using FlashThunder.Gameplay.Resources;
using Microsoft.Xna.Framework;
using FlashThunder.Defs;
using FlashThunder.Extensions;
namespace FlashThunder.Gameplay.Systems.OnUpdate.Input
{
    internal class PlayerDebuggingInputSystem : ISystem<float>
    {
        private readonly World _world;
        private List<IDisposable> _subscriptions;
        public bool IsEnabled { get; set; }

        public PlayerDebuggingInputSystem(World world)
        {
            _world = world;
            _subscriptions = [
                world.Subscribe<ActionActivatedEvent>(OnActionActivated)

                ];
        }

        public void OnActionActivated(in ActionActivatedEvent msg)
        {
            if (msg.action == GameAction.SpawnTest)
            {
                var mouseTile = _world.TileOfMouse();
                _world.RequestSpawn(EntityID.InfantryScout, mouseTile.X, mouseTile.Y);
            }
        }

        public void Update(float dt)
        {

        }
        public void Dispose() 
        { 
            _subscriptions.ForEach(s => s.Dispose());
        }
    }
}
