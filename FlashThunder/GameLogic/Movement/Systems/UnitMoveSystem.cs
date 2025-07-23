using fennecs;
using Microsoft.Xna.Framework;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.GameLogic.Selection.Components;

namespace FlashThunder.GameLogic.Movement.Systems
{
    internal sealed class UnitMoveSystem : IUpdateSystem<float>
    {
        private readonly Stream<MovableTiles, MoveIntent> _selectedAndMovableEntities;
        private readonly World _world;

        public UnitMoveSystem(World world)
        {
            _selectedAndMovableEntities = world.Query<MovableTiles, MoveIntent>()
                .Has<SelectedTag>()
                .Stream();

            _world = world;
        }

        public void Update(float upd)
        {
            var input = _world.GetResource<InputResource>();
            // if select action didn't happen, don't do anything
            if (!input.WasJustActivated(GameAction.Action))
                return;

            var mouse = _world.GetResource<MouseResource>();
            var mousePos = new Point(mouse.TileX, mouse.TileY);

            // - - - [ figure out whether the tile is valid ] - - -
            _selectedAndMovableEntities.For(
                (ref MovableTiles tiles, ref MoveIntent intent) =>
                {
                    if(tiles.Tiles.TryGetValue(mousePos, out var waypoints))
                    {
                        intent.Waypoints = waypoints;
                    }
                });
        }

        public void Dispose() { }
    }
}