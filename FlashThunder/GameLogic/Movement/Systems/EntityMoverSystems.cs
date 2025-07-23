using fennecs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Movement.Services;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Movement.Systems
{
    internal sealed class EntityMoverSystems : IUpdateSystem<float>
    {
        private readonly PathfindingService _pathfindingService;
        private readonly Stream<MoveCapable, MoveIntent, GridPosition>
            _readyToFollowEntities,
            _needsRangeRefresh;
        private readonly Stream<WaypointDebounce> _moveCooldowns;
        private readonly Stream<MoveIntent> _currentlyMovingEntities;
        public EntityMoverSystems(World world)
        {
            _pathfindingService = world.GetResource<PathfindingService>();

            // the systems interacting with movement should not interact with entities that are
            // currently moving
            var baseMovable = world.Query<MoveCapable, MoveIntent, GridPosition>();

            _readyToFollowEntities = baseMovable
                .Not<WaypointDebounce>()
                .Stream();

            _needsRangeRefresh = baseMovable
                .Not<MovableTiles>()
                .Not<MoveInProgressTag>()
                .Stream();

            _moveCooldowns = world.Query<WaypointDebounce>()
                .Stream();

            _currentlyMovingEntities = world.Query<MoveIntent>()
                .Has<MoveInProgressTag>()
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

                    // remove the entity's own tile
                    pathMap.Remove(new Point(pos.X, pos.Y));
                    
                    e.Add(new MovableTiles { Tiles = pathMap });
                });
        }

        private void ProcessMoveIntentSystem()
        {
            _readyToFollowEntities.For(
                (in Entity e, ref MoveCapable moveStats, ref MoveIntent moveIntent, ref GridPosition pos) =>
                {
                    if (moveIntent.Waypoints.Count > 0)
                    {
                        GridPosition nextPos;
                        // we skip waypoints that are the same as the current position
                        do
                        {
                            nextPos = moveIntent.Waypoints[0];
                            moveIntent.Waypoints.RemoveAt(0);
                        } while (moveIntent.Waypoints.Count > 0 && nextPos.X == pos.X && nextPos.Y == pos.Y);

                        pos.X = nextPos.X;
                        pos.Y = nextPos.Y;

                        e.Add(new WaypointDebounce { Value = moveStats.ProcessWaypointCD });

                        // if we haven't checked off as moving, check that off now
                        if (!e.Has<MoveInProgressTag>())
                        {
                            e.Remove<MovableTiles>();
                            e.Add<MoveInProgressTag>();
                        }
                    }
                });
        }

        private void MoveCDDisposalSystem(float dt)
        {
            _moveCooldowns.For(
                (in Entity e, ref WaypointDebounce moveCD) =>
            {
                moveCD.Value -= dt;
                if (moveCD.Value <= 0)
                    e.Remove<WaypointDebounce>();
            });
        }

        private void EndMoveSystem()
        {
            _currentlyMovingEntities.For(
                (in Entity e, ref MoveIntent moveIntent) =>
                {
                if (moveIntent.Waypoints.Count == 0)
                {
                    e.Remove<MoveInProgressTag>();
                }
            });
        }

        public void Update(float dt)
        {
            ProcessMoveIntentSystem();
            MovableTilesRefreshSystem();

            MoveCDDisposalSystem(dt);
            EndMoveSystem();
        }

        public void Dispose() { }
    }
}
