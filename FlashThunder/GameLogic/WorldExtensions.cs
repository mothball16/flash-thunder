using Microsoft.Xna.Framework;
using FlashThunder.ECSGameLogic.Components;
using System;
using fennecs;
using FlashThunder.Defs;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using FlashThunder.GameLogic;
using FlashThunder.Managers;

namespace FlashThunder.Extensions;

#region  - - - [ resources ] - - -
file struct UniqueResourceTag;

#endregion

/// <summary>
/// this pattern "borrowed" from some convo in fennecs discord in 2024
/// </summary>
public static class WorldExtensions
{
    private static Entity GetResourceEntity(this World world)
    {
        var query = world.Query<UniqueResourceTag>().Compile();
        if (query.IsEmpty)
        {
            world.Entity().Add<UniqueResourceTag>().Spawn();
        }
        return query[0];
    }
    public static ref T GetResource<T>(this World world)
        => ref world.GetResourceEntity().Ref<T>();

    public static bool TryGetResource<T>(this World world, out T resource)
    {
        if (world.GetResourceEntity().Has<T>())
        {
            resource = world.GetResource<T>();
            return true;
        }
        resource = default;
        return false;
    }

    public static void SetResource<T>(this World world, T resource)
        => world.GetResourceEntity().Add<T>(resource);

    public static void SetResource<T>(this World world) where T : new()
        => world.SetResource(new T());
    internal static EventBus GetEvents(this World world)
    {
        if (world.TryGetResource<EventBus>(out var eventBus))
            return eventBus;

        eventBus = new EventBus();
        world.SetResource(eventBus);
        return eventBus;
    }
}