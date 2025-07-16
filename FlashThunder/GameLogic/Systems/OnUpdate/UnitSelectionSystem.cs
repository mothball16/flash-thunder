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

namespace FlashThunder.GameLogic.Systems.OnUpdate
{
    internal sealed class UnitSelectionSystem(World world) : IUpdateSystem<float>
    {
        private readonly Stream<GridPosition> _query
            = world.Query<GridPosition>()
            .Has<SelectableTag>()
            .Stream();
        private readonly World _world = world;
        public void Update(float upd)
        {
            var input = _world.GetResource<InputResource>();
            if (!input.JustActivated.Contains(GameAction.Select))
                return;
            var mouse = _world.GetResource<MouseResource>();
            Logger.Print($"{mouse}");

            _query.For((in Entity e, ref GridPosition pos) =>
            {
                if(pos.X == mouse.TileX && pos.Y == mouse.TileY)
                {
                    Logger.Confirm($"Unit found on tile {pos.X}, {pos.Y}");
                }
            });
        }

        public void Dispose() { }
    }
}