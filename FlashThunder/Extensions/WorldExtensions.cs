using Microsoft.Xna.Framework;
using DefaultEcs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using System;
using FlashThunder.Defs;

namespace FlashThunder.Extensions;

public static class WorldExtensions
{

    public static void RequestSpawn(this World world, string entityID, int x, int y)
        => RequestSpawn(world, entityID, x, y, world.Get<TurnOrderResource>().Current);

    public static void RequestSpawn(this World world, string entityID, int x, int y, Entity owner)
    {
        var request = world.CreateEntity();
        request.Set(new SpawnRequestComponent(
            entityID,
            x,
            y,
            owner
        ));
        if (entityID != EntityID.ControlMarker)
        {
            Console.WriteLine($"{request} requested to create {entityID} on {x},{y}. has component {request.Get<SpawnRequestComponent>().EntityID}");
        }
    }

    public static void RequestNextTurn(this World world)
    {
        var request = world.CreateEntity();
        request.Set(new NextTurnRequestComponent());
    }

    public static void AddDebris(this World world, Entity e)
    {
        e.Set(new DestroyRequestComponent());
    }

    public static void AddDebris(this World world, Entity e, int frames)
    {
        e.Set(new DestroyInFramesRequestComponent { Lifetime = frames });
    }

    public static void AddDebris(this World world, Entity e, float seconds)
    {
        e.Set(new DestroyInSecondsRequestComponent { Lifetime = seconds });
    }
}