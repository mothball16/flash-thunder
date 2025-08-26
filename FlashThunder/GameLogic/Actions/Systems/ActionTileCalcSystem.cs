using fennecs;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Movement.Services;
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

        public ActionTileCalcSystem(World world)
        {
            _pathfindingService = world.GetResource<PathfindingService>();
            _needsRangeRefresh = world.Query<GridPosition, SkillSet, AbilitySelected>()
                .Not<ActionTiles>() // no need to refresh if never requested (by deleting action tiles)
                .Not<MoveInProgressTag>() // no need to refresh if we are mid-move (unable to act anyways)
                .Not<ExecutingActionTag>() // no need to refresh if we are mid-action
                .Stream();
        }

        public override void Update(float upd)
        {
            _needsRangeRefresh.For(
                (in Entity e, ref GridPosition pos, ref SkillSet skillSet, ref AbilitySelected selected) =>
            {
                var skill = skillSet[selected.AbilityIndex];
                switch (skill.SelectionType)
                {
                    case Enums.SelectionType.Pathfinding:
                        var from = new Point(pos.X, pos.Y);
                        var range = e.Ref<MoveCapable>().Range;
                        var traverse = e.Ref<MoveCapable>().Traverse;
                        /* can drop something here for future overridability -- not right now tho */

                        var pathMap = _pathfindingService.GetPathMap(from, range, traverse);

                        // remove the entity's own tile
                        pathMap.Remove(new Point(pos.X, pos.Y));

                        e.Add(new ActionTiles { Tiles = pathMap });
                        break;

                    case Enums.SelectionType.Self:
                        break;

                    case Enums.SelectionType.Friendlies:
                        break;

                    case Enums.SelectionType.Enemies:
                        break;

                    case Enums.SelectionType.Passthrough:
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Logger.Print($"re-calculating movable tiles for entity {e.GetHashCode()}");
            });
        }
    }
}