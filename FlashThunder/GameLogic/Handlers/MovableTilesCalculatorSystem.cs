using fennecs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using static System.Formats.Asn1.AsnWriter;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Utilities;
using FlashThunder.Extensions;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Resources;
using FlashThunder.GameLogic.Components;
using FlashThunder.Core;
using Microsoft.Xna.Framework.Input;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Services;

namespace FlashThunder.GameLogic.Handlers
{
    /// <summary>
    /// A system that acts on movable entities that need their movable tiles calculated.
    /// Recalculation is done by deleting the component. The system will pick this up during the
    /// next cycle.
    /// </summary>
    internal sealed class MovableTilesCalculatorSystem : IUpdateSystem<float>
    {
        private readonly Stream<MoveCapable, GridPosition> _movable;
        private readonly PathfindingService _pathfindingService;

        public MovableTilesCalculatorSystem(World world)
        {
            _movable = world.Query<MoveCapable, GridPosition>()
                .Not<MovableTiles>()
                .Stream();
            _pathfindingService = world.GetResource<PathfindingService>();
        }

        public void Update(float upd)
        {
            /* 
            this will set the MovableTiles component of the entity to the calculated path map
            (should just be one, but this "could" handle multiple entities
            */
            _movable.For(
                (in Entity e, ref MoveCapable moveStats, ref GridPosition pos) =>
                {
                    Logger.Print($"re-calculating movable tiles for entity {e.GetHashCode()}");
                    var pathMap = _pathfindingService.GetPathMap(
                        from: new Point(pos.X, pos.Y),
                        range: moveStats.Range,
                        canTraverse: moveStats.Traverse);
                    pathMap.Remove(new Point(pos.X, pos.Y)); // remove the entity's own tile
                    e.Add(new MovableTiles{Tiles = pathMap});
                });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}