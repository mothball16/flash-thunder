using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input
{
    /// <summary>
    /// Manages and disposes of entities marked for deletion.
    /// </summary>
    internal sealed class EntityDisposalTickerSystem : ISystem<float>
    {
        private readonly EntitySet _entitySetFrames;
        private readonly EntitySet _entitySetSeconds;

        public bool IsEnabled { get; set; }

        public EntityDisposalTickerSystem(World world)
        {
            _entitySetFrames = world.GetEntities()
                .With<ToDestroyInFramesComponent>()
                .AsSet();
            _entitySetSeconds = world.GetEntities()
                .With<ToDestroyInSecondsComponent>()
                .AsSet();
        }

        public void Update(float dt)
        {
            foreach (var e in _entitySetFrames.GetEntities())
            {
                ref var destroyComp = ref e.Get<ToDestroyInFramesComponent>();
                if (destroyComp.Lifetime <= 0)
                    e.Set(new ToDestroyComponent());

                destroyComp.Lifetime--;
            }

            foreach (var e in _entitySetSeconds.GetEntities())
            {
                ref var destroyComp = ref e.Get<ToDestroyInSecondsComponent>();
                if (destroyComp.Lifetime <= 0)
                    e.Set(new ToDestroyComponent());

                destroyComp.Lifetime -= dt;
            }
        }

        public void Dispose() { }
    }
}