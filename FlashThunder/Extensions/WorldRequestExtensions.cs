using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DefaultEcs;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
namespace FlashThunder.Extensions
{
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

        public static Point TileOfMouse(this World world)
        {
            var mouseResource = world.Get<MouseResource>();
            var tileSize = world.Get<EnvironmentResource>().TileSize;

            return new Point(mouseResource.WorldX / tileSize, mouseResource.WorldY / tileSize);
        }
    }
}
