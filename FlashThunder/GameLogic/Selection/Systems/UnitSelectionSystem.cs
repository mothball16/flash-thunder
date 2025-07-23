using fennecs;
using FlashThunder.Enums;
using FlashThunder.Core;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.GameLogic.CameraControl.Requests;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Components;

namespace FlashThunder.GameLogic.Unit.Systems
{
    internal sealed class UnitSelectionSystem : IUpdateSystem<float>
    {
        private const int TileSize = GameConstants.TileSize;
        private readonly Stream<GridPosition> _isSelectable;
        private readonly Stream<SelectedTag> _alreadySelected;

        private readonly World _world;

        public UnitSelectionSystem(World world)
        {
            _isSelectable = world.Query<GridPosition>()
                .Has<SelectableTag>()
                .Stream();
            _alreadySelected = world.Query<SelectedTag>()
                .Stream();
            _world = world;
        }

        private void DeselectUnit()
            => _alreadySelected.For((in Entity e, ref SelectedTag _) => e.Remove<SelectedTag>());

        /// <summary>
        /// The operation that this system performs on the selected entity.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pos"></param>
        private void SelectUnit(Entity e, GridPosition pos)
        {
            DeselectUnit();
            e.Add<SelectedTag>();

            _world.Publish(new CamTranslationRequest(
                pos.X * TileSize + TileSize / 2,
                pos.Y * TileSize + TileSize / 2,
                OffsetType: CamOffsetType.Absolute));
        }

        public void Update(float upd)
        {
            // - - - [ edge handling ] - - -

            var input = _world.GetResource<InputResource>();
            if (input.WasJustActivated(GameAction.Deselect))
            {
                DeselectUnit();
                return;
            }

            // we should not be able to select more than one unit
            if (_alreadySelected.Count > 0)
                return;

            // if select action didn't happen, don't do anything
            if (!input.WasJustActivated(GameAction.Action))
                return;

            // - - - [ figure out what unit we are trying to select ] - - -
            var mouse = _world.GetResource<MouseResource>();

            _isSelectable.For((in Entity e, ref GridPosition pos) =>
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