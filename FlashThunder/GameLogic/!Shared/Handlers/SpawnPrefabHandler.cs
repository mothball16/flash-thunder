﻿using fennecs;
using FlashThunder.Factories;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.TeamLogic.Services;
using System;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Commands;

internal sealed class SpawnPrefabHandler : IDisposable
{
    private readonly EntityFactory _factory;
    private readonly List<IDisposable> _subscriptions;
    private readonly TeamService _teamService;
    public SpawnPrefabHandler(World world, EntityFactory factory)
    {
        _factory = factory;
        _subscriptions = [
            world.Subscribe<SpawnPrefabRequest>(Execute)
            ];
        _teamService = world.GetResource<TeamService>();
    }

    public void Execute(SpawnPrefabRequest msg)
    {
        var entity = _factory.CreatePrefab(msg.Name);

        if (msg.Position != null)
            entity.Add<GridPosition>(msg.Position.Value);

        if (msg.Team != null)
            _teamService.AssignTeam(entity, msg.Team);

        msg.Callback?.Invoke(entity);
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
    }
}
