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

internal sealed class PlayerDebuggingInputSystem : AStandardSystem<GameFrameSnapshot>
{
    private readonly List<IDisposable> _subscriptions;

    public PlayerDebuggingInputSystem(World world) : base(world)
    {

        _subscriptions = [
            world.Subscribe<ActionActivatedEvent>(OnActionActivated)
            ];
    }

    public void OnActionActivated(in ActionActivatedEvent msg)
    {
        if (msg.Action == GameAction.SpawnTest)
        {
            var mouseTile = msg.Mouse.TilePosition;
            World.RequestSpawn(EntityID.InfantryScout, mouseTile.X, mouseTile.Y);
        } 
        else if(msg.Action == GameAction.EndTurn)
        {
            World.RequestNextTurn();
        }
    }

    public override void Update(GameFrameSnapshot _)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _subscriptions.ForEach(s => s.Dispose());
        }
        base.Dispose(disposing);
    }
}