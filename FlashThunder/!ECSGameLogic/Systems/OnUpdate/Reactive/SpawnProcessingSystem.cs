namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Set.Reactive;
using System;
using DefaultEcs;
using DefaultEcs.Command;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder._ECSGameLogic.Components;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.Factories;

[With(typeof(SpawnRequestComponent))]
internal sealed class SpawnProcessingSystem : AStandardSystem<GameFrameSnapshot>
{
    private readonly EntityFactory _entityFactory;
    private readonly EntitySet _set;

    public SpawnProcessingSystem(World world, EntityFactory factory) : base(world)
    {
        _entityFactory = factory;
        _set = world.GetEntities()
            .With<SpawnRequestComponent>()
            .AsSet();
    }

    public override void Update(GameFrameSnapshot state)
    {
        Console.WriteLine("------");
        foreach (ref readonly Entity entity in _set.GetEntities())
        {
            var requestInfo = entity.Get<SpawnRequestComponent>();
            Console.WriteLine($"{entity} - received request to spawn {requestInfo.EntityID} on tile {requestInfo.X},{requestInfo.Y}");

            var newEntity = _entityFactory.CreateEntity(requestInfo.EntityID, requestInfo.X, requestInfo.Y);
            newEntity.Set(new OwnableComponent
            {
                Owner = requestInfo.Owner
            });
            newEntity.Set(new IdentifierComponent
            {
                ID = requestInfo.EntityID
            });
            World.AddDebris(entity);
        }
    }
}