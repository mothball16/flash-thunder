namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Reactive;
using System;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.Factories;

internal sealed class SpawnProcessingSystem : ISystem<float>
{
    private readonly World _world;
    private readonly EntitySet _entitySet;
    private readonly EntityFactory _entityFactory;

    public bool IsEnabled { get; set; }

    public SpawnProcessingSystem(World world, EntityFactory factory)
    {
        _world = world;
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
            _world.AddDebris(request);
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}