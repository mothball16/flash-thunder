using fennecs;
using FlashThunder.GameLogic.Movement.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic._Shared.Services
{
    internal class LookupService
    {
        private Dictionary<Point, HashSet<Entity>> _entityMultiMap;
        private readonly Stream<GridPosition> _entitiesWithPos;
        public LookupService(World world)
        {
            _entityMultiMap = [];
            _entitiesWithPos = world.Query<GridPosition>().Stream();
        }

        // TODO: not a priority, but optimize in the future with a multimap. right now this just loops
        // through everything
        public List<Entity> EntitiesOnTile(Point target)
        {
            List<Entity> entities = [];
            _entitiesWithPos.For(
                (in Entity e, ref GridPosition pos) =>
                {
                    if(pos.X == target.X && pos.Y == target.Y)
                    {
                        entities.Add(e);
                    }
                });
            return entities;
        }
    }
}
