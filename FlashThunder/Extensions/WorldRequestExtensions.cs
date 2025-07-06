using Microsoft.Xna.Framework;
using DefaultEcs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using System;

namespace FlashThunder.Extensions;

public static class WorldRequestExtensions
{
    public static void RequestSpawn(this World world, string enemyID, int x, int y)
    {
        var request = world.CreateEntity();

        request.Set(new SpawnRequestComponent
        {
            EntityID = enemyID,
            X = x,
            Y = y
        });
    }

    public static void AddDebris(this World world, Entity e)
    {
        e.Set(new ToDestroyComponent());
    }
    public static void AddDebris(this World world, Entity e, int frames)
    {
        e.Set(new ToDestroyInFramesComponent { Lifetime = frames});
    }
    public static void AddDebris(this World world, Entity e, float seconds)
    {
        e.Set(new ToDestroyInSecondsComponent { Lifetime = seconds });
    }
}