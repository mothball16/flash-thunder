using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Components;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input
{
    /// <summary>
    /// Modifies the intents of all controlled units.
    /// </summary>
    internal sealed class PlayerCommandSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntitySet _entitySet;

        public bool IsEnabled { get; set; }

        public PlayerCommandSystem(World world)
        {
            _world = world;

            _entitySet = world.GetEntities()
                .With<ControlledComponent>()
                .With<GridPosComponent>()
                .AsSet();
        }

        public void Update(float dt)
        {
            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {
            }
        }

        public void Dispose() { }
    }
}