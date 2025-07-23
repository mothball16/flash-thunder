using fennecs;
using FlashThunder.Core;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Movement.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Movement.Systems
{
    /// <summary>
    /// WorldPosAutoAdderSystem -> automatically adds a WorldPosition component to entities with a WorldToGridMover
    /// WorldToGridLerpSystem -> handles the lerping of WorldPosition to GridPosition for entities with a WorldToGridMover
    /// </summary>
    internal sealed class WorldToGridAnimatorSystem(World world) : IUpdateSystem<float>
    {
        private const int t = GameConstants.TileSize;
        private readonly Stream<WorldPosition, GridPosition, WorldToGridAnimator> _WTGanimatedEntities
            = world.Query<WorldPosition, GridPosition, WorldToGridAnimator>().Stream();

        private readonly Stream<GridPosition> _needsAWorldPosition
            = world.Query<GridPosition>()
            .Has<WorldToGridAnimator>()
            .Not<WorldPosition>()
            .Stream();

        private static float NumLerp(float a, float b, float t)
            => a + (b - a) * t;

        /// <summary>
        /// Adds a worldposition component to entities with a WorldToGridAnimator if not already
        /// assigned. The worldposition wlil in 99% of cases just be the grid position on
        /// initialization  this is handled here.
        /// </summary>
        private void WorldPosAutoAdderSystem()
        {
            _needsAWorldPosition.For((in Entity e, ref GridPosition pos) 
                => e.Add(new WorldPosition(pos.X * t, pos.Y * t)));
        }

        private void WorldMoveToGridPosSystem(float dt)
        {
            _WTGanimatedEntities.For(
                (ref WorldPosition worldPos, ref GridPosition gridPos, ref WorldToGridAnimator mover) =>
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
