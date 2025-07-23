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
using FlashThunder.GameLogic.Components;
using FlashThunder.Core;
using Microsoft.Xna.Framework.Input;
using FlashThunder.GameLogic.Events;
using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;

namespace FlashThunder.GameLogic.Movement.Systems
{
    internal sealed class UnitMoveSystem : IUpdateSystem<float>
    {
        private readonly Stream<MovableTiles, MoveIntent> _selectedAndMovableEntities;
        private readonly World _world;

        public UnitMoveSystem(World world)
        {
            _selectedAndMovableEntities = world.Query<MovableTiles, MoveIntent>()
                .Has<IsPlayerControllable>()
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