namespace FlashThunder.Gameplay.Systems.OnUpdate.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core;
using FlashThunder.Defs;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;

internal sealed class SpawnProcessingSystem : ISystem<float>
{
    private readonly EntitySet _entitySet;
    private readonly EntityFactory _entityFactory;

    public bool IsEnabled { get; set; }

    public SpawnProcessingSystem(World world, EntityFactory factory)
    {
        _entityFactory = factory;

        _entitySet = world.GetEntities()
            .With<SpawnRequestComponent>()
            .AsSet();
    }

    public void Update(float dt)
    {
        foreach (ref readonly Entity request in _entitySet.GetEntities())
        {
            var requestInfo = request.Get<SpawnRequestComponent>();
            _entityFactory.CreateEntity(requestInfo.EntityID, requestInfo.X, requestInfo.Y);

            if (requestInfo.EntityID == EntityID.TestEntity)
            {
                Console.WriteLine(
                    $"{requestInfo.EntityID} was requested @ {requestInfo.X}, {requestInfo.Y}");
            }

            request.Dispose();
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}