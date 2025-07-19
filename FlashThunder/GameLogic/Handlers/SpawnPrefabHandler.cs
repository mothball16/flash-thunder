using fennecs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.Factories;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
