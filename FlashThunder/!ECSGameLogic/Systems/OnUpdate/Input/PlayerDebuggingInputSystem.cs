using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.Enums;
using FlashThunder.Extensions;
using System;
using System.Collections.Generic;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

internal sealed class PlayerDebuggingInputSystem : ISystem<GameFrameSnapshot>
{
    private readonly World _world;
    private readonly List<IDisposable> _subscriptions;
    private GameFrameSnapshot _lastSnapshot;
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
            var mouseTile = _lastSnapshot.Mouse.TilePosition;
            _world.RequestSpawn(EntityID.InfantryScout, mouseTile.X, mouseTile.Y);
        }
    }

    public void Update(GameFrameSnapshot state)
    {
        _lastSnapshot = state;
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
        GC.SuppressFinalize(this);
    }
}