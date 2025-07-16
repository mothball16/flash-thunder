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
using FlashThunder.GameLogic.Events;

namespace FlashThunder.Extensions;

#region - - - [ resources ] - - -

internal struct UniqueResourceTag;

#endregion - - - [ resources ] - - -

/// <summary>
/// this pattern "borrowed" from some convo in fennecs discord in 2024
/// </summary>
public static class WorldExtensions
{
    private static Entity GetResourceEntity(this World world)
    {
        return world.Query<UniqueResourceTag>().Compile()[0];
    }

    /// <summary>
    /// Must be called so extensions don't need to check whether certain resources are 
    /// initialized.
    /// </summary>
    /// <param name="world"></param>
    public static World InitializeExtensions(this World world)
    {
        // init the resource entity
        world.Entity().Add<UniqueResourceTag>().Spawn();

        // init the event bus
        var eventBus = new EventBus();
        world.SetResource(eventBus);
        return world;
    }

    #region - - - [ resources ] - - -
    public static ref T GetResource<T>(this World world)
        => ref world.GetResourceEntity().Ref<T>();

    public static void SetResource<T>(this World world, T resource)
    {
        var resources = world.GetResourceEntity();
        if (resources.Has<T>())
        {
            resources.Ref<T>() = resource;
            return;
        }

        world.GetResourceEntity().Add(resource);
    }

    public static void SetResource<T>(this World world) where T : new()
        => world.SetResource(new T());
    #endregion

    #region - - - [ events ] - - -
    internal static EventBus GetEvents(this World world)
        => world.GetResource<EventBus>();
    internal static void Publish<T>(this World world, T data)
        => world.GetEvents().Publish(data);
    internal static IDisposable Subscribe<T>(this World world, Action<T> handler)
        => world.GetEvents().Subscribe(handler);
    #endregion
}