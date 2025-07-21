using fennecs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Services;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Systems.OnUpdate
{
    /// <summary>
    /// MovableTilesRefreshSystem -> handles the recalculation of movable tiles for entities that have moved<br/>
    /// MoveToNextWaypointSystem -> moves entities to their next waypoint if they have one in their MoveIntent<br/>
    /// MoveCDTickerSystem -> updates the value of MoveCD components, removing them when they reach zero<br/>
    /// </summary>
    internal sealed class EntityMoverSystems : IUpdateSystem<float>
    {
        private readonly PathfindingService _pathfindingService;
        private readonly Stream<MoveCapable, MoveIntent, GridPosition> _ableToMove, _needsRangeRefresh;
        private readonly Stream<MoveCD> _moveCooldowns;
        public EntityMoverSystems(World world)
        {
            _pathfindingService = world.GetResource<PathfindingService>();
            var baseMovable = world.Query<MoveCapable, MoveIntent, GridPosition>().Not<MoveCD>();
            _ableToMove = baseMovable
                .Stream();
            _needsRangeRefresh = baseMovable.Not<MovableTiles>()
                .Stream();
            _moveCooldowns = world.Query<MoveCD>()
                .Stream();
        }
        /*
        this will set the MovableTiles component of the entity to the calculated path map
        (should just be one, but this "could" handle multiple entities
        */
        private void MovableTilesRefreshSystem()
        {
            _needsRangeRefresh.For(
                (in Entity e, ref MoveCapable moveStats, ref MoveIntent _, ref GridPosition pos) =>
                {
                    Logger.Print($"re-calculating movable tiles for entity {e.GetHashCode()}");
                    var pathMap = _pathfindingService.GetPathMap(
                        from: new Point(pos.X, pos.Y),
                        range: moveStats.Range,
                        canTraverse: moveStats.Traverse);
                    pathMap.Remove(new Point(pos.X, pos.Y)); // remove the entity's own tile
                    e.Add(new MovableTiles { Tiles = pathMap });
                });
        }

        private void MoveToNextWaypointSystem()
        {
            _ableToMove.For(
                (in Entity e, ref MoveCapable moveStats, ref MoveIntent moveIntent, ref GridPosition pos) =>
                {
                    if (moveIntent.Waypoints.Count > 0)
                    {
                        var moving = !e.Has<MovableTiles>();

                        // if we are just beginning to move, then we should only set a temporary CD
                        // to indicate that movabletiles should not be rebuilt on the next frame
                        e.Add(new MoveCD { Value = !moving ? 0 : moveStats.ProcessWaypointCD });
                        
                        pos = moveIntent.Waypoints[0];
                        moveIntent.Waypoints.RemoveAt(0);

                        // we want to recalc the movable tiles
                        if (!moving)
                            e.Remove<MovableTiles>();
                    }
                });
        }

        private void MoveCDTickerSystem(float dt)
        {
            _moveCooldowns.For(
                (in Entity e, ref MoveCD moveCD) =>
            {
                moveCD.Value -= dt;
                if (moveCD.Value <= 0)
                    e.Remove<MoveCD>();
            });
        }

        public void Update(float dt)
        {
            MoveToNextWaypointSystem();
            MovableTilesRefreshSystem();
            MoveCDTickerSystem(dt);
        }

        public void Dispose() { }
    }
}
