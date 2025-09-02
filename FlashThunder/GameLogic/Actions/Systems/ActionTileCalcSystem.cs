using fennecs;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Movement.Services;
using FlashThunder.GameLogic.Resources;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Actions.Systems
{
    internal class ActionTileCalcSystem : AUpdateSystem<float>
    {
        private readonly PathfindingService _pathfindingService;
        private readonly Stream<GridPosition, SkillSet, AbilitySelected> _needsRangeRefresh;
        private readonly World _world;
        public ActionTileCalcSystem(World world)
        {
            _pathfindingService = world.GetResource<PathfindingService>();
            _needsRangeRefresh = world.Query<GridPosition, SkillSet, AbilitySelected>()
                .Not<ActionTiles>() // no need to refresh if never requested (by deleting action tiles)
                .Not<MoveInProgressTag>() // no need to refresh if we are mid-move (unable to act anyways)
                .Not<ExecutingActionTag>() // no need to refresh if we are mid-action
                .Stream();
            _world = world;
        }


        #region Selection Type Methods
        private Dictionary<Point, List<Point>> CalcPathfindingTiles(GridPosition pos, UnitSkill skill)
        {
            Logger.Warn("This should eventually be moved out of MoveCapable and into a regular skill");
            var from = new Point(pos.X, pos.Y);
            var range = skill.Range;
            var traverse = skill.Traverse;
            /* can drop something here for future overridability -- not right now tho */

            var pathMap = _pathfindingService.GetPathMap(from, range, traverse);

            // remove the entity's own tile
            pathMap.Remove(new Point(pos.X, pos.Y));

            return pathMap;
        }

        private static Dictionary<Point, List<Point>> CalcSelfTiles(GridPosition pos)
        {
            var tiles = new Dictionary<Point, List<Point>>()
                {
                    {new Point(pos.X, pos.Y), default }
                };
            return tiles;
        }

        private static Dictionary<Point, List<Point>> CalcLOSTiles(GridPosition pos, UnitSkill skill)
        {
            var tiles = new Dictionary<Point, List<Point>>();
            throw new NotImplementedException();
        }

        private Dictionary<Point, List<Point>> CalcPassthroughTiles(GridPosition pos, UnitSkill skill)
        {
            var map = _world.GetResource<MapResource>();
            var tiles = new Dictionary<Point, List<Point>>();
            for (int row = -skill.Range; row <= skill.Range; row++)
            {
                for (int col = -skill.Range; col <= skill.Range; col++)
                {
                    int dist = Math.Abs(row) + Math.Abs(col);
                    var tilePos = new Point(pos.X + col, pos.Y + row);
                    if (dist <= skill.Range 
                        && tilePos.X >= 0 && tilePos.X < map.Width 
                        && tilePos.Y >= 0 && tilePos.Y < map.Height
                        && _pathfindingService.IsPassable(tilePos, skill.Traverse, 1))
                    {
                        tiles.Add(tilePos, default);
                    }
                }
            }
            return tiles;
        }

        #endregion

        public override void Update(float upd)
        {
            _needsRangeRefresh.For(
                (in Entity e, ref GridPosition pos, ref SkillSet skillSet, ref AbilitySelected selected) =>
            {
                var skill = skillSet[selected.AbilityIndex].Data;
                Dictionary<Point, List<Point>> tiles;
                switch (skill.SelectionType)
                {
                    case Enums.SelectionType.Pathfinding:
                        tiles = CalcPathfindingTiles(pos, skill); break;
                    case Enums.SelectionType.Self:
                        tiles = CalcSelfTiles(pos); break;
                    case Enums.SelectionType.LineOfSight:
                        tiles = CalcLOSTiles(pos, skill); break;
                    case Enums.SelectionType.Passthrough:
                        tiles = CalcPassthroughTiles(pos, skill); break;

                    default:
                        throw new NotImplementedException();
                }
                e.Add(new ActionTiles { Tiles = tiles});

                Logger.Print($"re-calculating movable tiles for entity {e.GetHashCode()}");
            });
        }
    }
}