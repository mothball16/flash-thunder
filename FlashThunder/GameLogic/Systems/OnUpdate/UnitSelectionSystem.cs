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
        private readonly Stream<GridPosition> _selectable
            = world.Query<GridPosition>()
            .Has<SelectableTag>()
            .Stream();

        private readonly Stream<SelectedTag> _curSelected
            = world.Query<SelectedTag>().Stream();

        private readonly World _world = world;

        private void ClearSelection()
            => _curSelected.For((in Entity e, ref SelectedTag _) => e.Remove<SelectedTag>());

        public void Update(float upd)
        {
            // - - - [ edge handling ] - - -
            var input = _world.GetResource<InputResource>();
            if (input.JustActivated.Contains(GameAction.Deselect))
            {
                ClearSelection();
                return;
            }
            // if select action didn't happen, don't do anything
            // ( consider moving this over to a handler )
            if (!input.JustActivated.Contains(GameAction.Select))
                return;

            // - - - [ the actual logic ] - - -

            // figure out what unit we are trying to select
            var mouse = _world.GetResource<MouseResource>();

            // add selected tag to any unit on that tile
            _selectable.For((in Entity e, ref GridPosition pos) =>
            {
                if(pos.X == mouse.TileX && pos.Y == mouse.TileY)
                {
                    ClearSelection();
                    e.Add<SelectedTag>();
                }
            });
        }

        public void Dispose() { }
    }
}