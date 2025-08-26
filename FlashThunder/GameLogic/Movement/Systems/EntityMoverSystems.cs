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
        private readonly Stream<MoveCapable, MoveIntent, GridPosition>
            _readyToFollowEntities;
        private readonly Stream<WaypointDebounce> _moveCooldowns;
        private readonly Stream<MoveIntent> _currentlyMovingEntities;
        public EntityMoverSystems(World world)
        {

            // the systems interacting with movement should not interact with entities that are
            // currently moving
            var baseMovable = world.Query<MoveCapable, MoveIntent, GridPosition>();

            _readyToFollowEntities = baseMovable
                .Not<WaypointDebounce>()
                .Stream();

            _moveCooldowns = world.Query<WaypointDebounce>()
                .Stream();

            _currentlyMovingEntities = world.Query<MoveIntent>()
                .Has<MoveInProgressTag>()
                .Stream();
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
