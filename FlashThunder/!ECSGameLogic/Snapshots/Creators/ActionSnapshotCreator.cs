using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.Enums;
using FlashThunder.Snapshots;
using Microsoft.Xna.Framework;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

internal sealed class ActionSnapshotCreator : IDisposable
{
    private readonly HashSet<GameAction> _actions;
    private readonly List<IDisposable> _subscriptions;

    public ActionSnapshotCreator(World world)
    {
        _actions = [];
        _subscriptions = [
            world.Subscribe<ActionActivatedEvent>(AddAction),
            world.Subscribe<ActionReleasedEvent>(RemoveAction)
        ];
    }
    private void AddAction(in ActionActivatedEvent msg)
    {
        _actions.Add(msg.Action);
    }

    private void RemoveAction(in ActionReleasedEvent msg)
    {
        _actions.Remove(msg.Action);
    }

    public ActionSnapshot GetSnapshot()
        => new(_actions);

    public void Dispose()
    {
        _subscriptions.ForEach(a => a.Dispose());
        GC.SuppressFinalize(this);
    }
}