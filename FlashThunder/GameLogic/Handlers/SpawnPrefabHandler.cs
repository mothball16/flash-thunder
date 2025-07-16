using fennecs;
using FlashThunder.Extensions;
using FlashThunder.Factories;
using FlashThunder.GameLogic.Events;
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
    public SpawnPrefabHandler(World world, EntityFactory factory)
    {
        _factory = factory;
        _subscriptions = [
            world.GetEvents().Subscribe<SpawnPrefabRequestEvent>(Execute)
            ];
    }

    public void Execute(SpawnPrefabRequestEvent msg)
    {
        var entity = _factory.CreatePrefab(msg.Name);
        msg.Callback?.Invoke(entity);
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
    }
}
