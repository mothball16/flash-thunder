using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Enums;
using FlashThunder.Defs;
using FlashThunder.Extensions;
using FlashThunder.ECSGameLogic.Events;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input
{
    internal sealed class PlayerDebuggingInputSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly List<IDisposable> _subscriptions;
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
            if (msg.Action == GameAction.SpawnTest)
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
            GC.SuppressFinalize(this);
        }
    }
}