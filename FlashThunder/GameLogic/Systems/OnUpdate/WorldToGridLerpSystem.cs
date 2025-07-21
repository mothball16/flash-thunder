using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Systems.OnUpdate
{
    /// <summary>
    /// WorldPosAutoAdderSystem -> automatically adds a WorldPosition component to entities with a WorldToGridMover
    /// WorldToGridLerpSystem -> handles the lerping of WorldPosition to GridPosition for entities with a WorldToGridMover
    /// </summary>
    internal sealed class WorldToGridLerpSystem(World world) : IUpdateSystem<float>
    {
        private const int t = GameConstants.TileSize;
        private readonly Stream<WorldPosition, GridPosition, WorldToGridMover> _query
            = world.Query<WorldPosition, GridPosition, WorldToGridMover>().Stream();

        private readonly Stream<GridPosition> _fresh
            = world.Query<GridPosition>()
            .Has<WorldToGridMover>()
            .Not<WorldPosition>()
            .Stream();

        private static float NumLerp(float a, float b, float t)
            => a + (b - a) * t;

        private void WorldPosAutoAdderSystem()
        {
            _fresh.For((in Entity e, ref GridPosition pos) 
                => e.Add(new WorldPosition(pos.X * t, pos.Y * t)));
        }

        private void WorldMoveToGridPosSystem(float dt)
        {
            _query.For(
                (ref WorldPosition worldPos, ref GridPosition gridPos, ref WorldToGridMover mover) =>
                {
                    float rate = 1 - MathF.Exp(-dt * mover.Response);
                    worldPos.X = NumLerp(worldPos.X, gridPos.X * t, rate);
                    worldPos.Y = NumLerp(worldPos.Y, gridPos.Y * t, rate);
                });
        }

        public void Update(float dt)
        {
            WorldPosAutoAdderSystem();
            WorldMoveToGridPosSystem(dt);
        }

        public void Dispose()
        {

        }
    }
}
