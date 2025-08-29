using fennecs;
using FlashThunder.GameLogic._Shared;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Movement.Services;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;

namespace FlashThunder.GameLogic.Movement.Systems
{
    internal sealed class EntityMoverSystems : AUpdateSystem<float>
    {
        private readonly Stream<MoveIntent, GridMover, GridPosition>
            _readyToFollowEntities;
        private readonly Stream<WaypointDebounce> _moveCooldowns;
        private readonly Stream<MoveIntent> _currentlyMovingEntities;
        public EntityMoverSystems(World world)
        {
            // entities ready to step to the next waypoint (debounce cleared)
            _readyToFollowEntities = world.Query<MoveIntent, GridMover, GridPosition>()
                .Not<WaypointDebounce>()
                .Stream();
            
            // entities currently with a move debounce waiting to be cleared
            _moveCooldowns = world.Query<WaypointDebounce>()
                .Stream();

            // entities that are currently moving (waypoint count > 0)
            _currentlyMovingEntities = world.Query<MoveIntent>()
                .Has<MoveInProgressTag>()
                .Stream();
        }

        private void ProcessMoveIntentSystem()
        {
            _readyToFollowEntities.For(
                (in Entity e, ref MoveIntent moveIntent, ref GridMover moveStats, ref GridPosition pos) =>
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

                        e.Add(new WaypointDebounce { Value = moveStats.WaypointCD});

                        // if we haven't checked off as moving, check that off now
                        if (!e.Has<MoveInProgressTag>())
                            e.Add<MoveInProgressTag>();
                        
                        e.TryRemove<ActionTiles>();
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

        public override void Update(float dt)
        {
            ProcessMoveIntentSystem();

            MoveCDDisposalSystem(dt);
            EndMoveSystem();
        }

    }
}
