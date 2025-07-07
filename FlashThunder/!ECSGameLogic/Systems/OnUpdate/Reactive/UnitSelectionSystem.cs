using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.Enums;
using FlashThunder.Extensions;
using System;
using System.Collections.Generic;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

/// <summary>
/// Modifies the intents of all controlled units.
/// </summary>
[With(typeof(SelectUnitsOnTileRequestComponent))]
internal sealed class UnitSelectionSystem : AEntitySetSystem<GameFrameSnapshot>
{
    private readonly List<IDisposable> _subscriptions;
    private readonly EntityMultiMap<GridPosComponent> _positions;
    private readonly TurnOrderResource _turnOrder;
    public bool IsEnabled { get; set; }

    public UnitSelectionSystem(World world) : base(world)
    {
        _positions = World.Get<EntityMultiMap<GridPosComponent>>();
        _turnOrder = World.Get<TurnOrderResource>();
        _subscriptions = [World.Subscribe<ActionActivatedEvent>(On)];
    }

    private void On(in ActionActivatedEvent msg)
    {
        if (msg.Action != GameAction.Select) return;
        var request = World.CreateEntity();
        request.Set(new SelectUnitsOnTileRequestComponent
        {
            X = msg.Mouse.TilePosition.X,
            Y = msg.Mouse.TilePosition.Y,
        });
    }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        var request = entity.Get<SelectUnitsOnTileRequestComponent>();
        var requestAsGridPos = new GridPosComponent(request.X, request.Y);

        if(_positions.TryGetEntities(requestAsGridPos, out var entities))
        {
            // If we have entities at the requested position, select them
            foreach (var e in entities)
            {
                bool isValidEntity =
                    e.Has<MoveComponent>()
                    && e.Has<OwnableComponent>()
                    && e.Get<OwnableComponent>().Owner == _turnOrder.Current;
                if (isValidEntity)
                {
                    // give them a selected component
                    e.Set<SelectedComponent>();
                } else
                {
                    Console.WriteLine($"Entity was not valid for selection.");
                }
            }
        }
        else
        {
            // No entities found at the requested position
            Console.WriteLine($"No controlled units found at {request.X}, {request.Y}");
        }

        World.AddDebris(entity);
    }

    public override void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
        GC.SuppressFinalize(this);
        base.Dispose();
    }
}