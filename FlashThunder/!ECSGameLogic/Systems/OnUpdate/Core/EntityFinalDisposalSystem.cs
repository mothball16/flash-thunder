using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Core
{
    /// <summary>
    /// Manages and disposes of entities marked for deletion.
    /// </summary>
    internal sealed class EntityFinalDisposalSystem : ISystem<float>
    {
        private readonly EntitySet _electricChair;

        public bool IsEnabled { get; set; }

        public EntityFinalDisposalSystem(World world)
        {
            _electricChair = world.GetEntities()
                .With<ToDestroyComponent>()
                .AsSet();
        }

        public void Update(float dt)
        {
            foreach (var e in _electricChair.GetEntities())
            {
                e.Dispose();
            }
        }

        public void Dispose() { }
    }
}