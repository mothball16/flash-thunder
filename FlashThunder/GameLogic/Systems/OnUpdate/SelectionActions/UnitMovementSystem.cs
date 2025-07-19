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

namespace FlashThunder.GameLogic.Systems.OnUpdate
{
    internal sealed class UnitMovementSystem : IUpdateSystem<float>
    {
        private const int TileSize = GameConstants.TileSize;
        private readonly Stream<SelectedTag, MoveCapable, MoveIntent, GridPosition>

        private readonly World _world;

        public UnitMovementSystem(World world)
        {
            _selectable = world.Query<GridPosition>()
                .Has<SelectableTag>()
                .Stream();
            _curSelected = world.Query<SelectedTag>()
                .Stream();
            _world = world;
        }

        private void ClearSelection()
            => _curSelected.For((in Entity e, ref SelectedTag _) => e.Remove<SelectedTag>());

        /// <summary>
        /// The operation that this system performs on the selected entity.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pos"></param>
        private void SelectUnit(Entity e, GridPosition pos)
        {
            ClearSelection();
            e.Add<SelectedTag>();

            _world.Publish(new CamTranslationRequest(
                pos.X + (TileSize / 2),
                pos.Y + (TileSize / 2),
                OffsetType: CamOffsetType.Absolute));
        }

        public void Update(float upd)
        {
            // - - - [ edge handling ] - - -

            var input = _world.GetResource<InputResource>();
            if (input.WasJustActivated(GameAction.Deselect))
            {
                ClearSelection();
                return;
            }

            // we should not be able to select more than one unit
            if (_curSelected.Count > 0)
                return;

            // if select action didn't happen, don't do anything
            if (!input.WasJustActivated(GameAction.Action))
                return;

            // - - - [ figure out what unit we are trying to select ] - - -
            var mouse = _world.GetResource<MouseResource>();

            _selectable.For((in Entity e, ref GridPosition pos) =>
            {
                if(pos.X == mouse.TileX && pos.Y == mouse.TileY)
                {
                    SelectUnit(e, pos);

                    // we don't want to handle multiple actions in one frame, so have it expire
                    input.UseAction(GameAction.Action);
                }
            });
        }

        public void Dispose() { }
    }
}