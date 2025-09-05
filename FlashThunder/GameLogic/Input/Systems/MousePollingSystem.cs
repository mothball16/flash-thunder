using Dcrew.MonoGame._2D_Camera;
using fennecs;
using FlashThunder.Core;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FlashThunder.GameLogic.Input.Systems
{
    internal sealed class MousePollingSystem(World world, Camera camera) : AUpdateSystem<float>
    {
        private const int TileSize = GameConstants.TileSize;
        private readonly World _world = world;
        private readonly Camera _camera = camera;
        private MouseState _lastMouseState = Mouse.GetState();

        public override void Update(float upd)
        {
            var mapResource = _world.Get<MapResource>();
            var mouseState = Mouse.GetState();
            var position = mouseState.Position;

            var mouseDiff = position - _lastMouseState.Position;
            var mouseDelta = (float) Math.Sqrt(
                mouseDiff.X * mouseDiff.X +
                mouseDiff.Y * mouseDiff.Y);
            float scrollDelta = mouseState.ScrollWheelValue - _lastMouseState.ScrollWheelValue;

            var worldPosition = _camera.ScreenToWorld(position);

            var tilePosition = new Point(
                Math.Clamp(worldPosition.X / TileSize, 0, mapResource.Width - 1),
                Math.Clamp(worldPosition.Y / TileSize, 0, mapResource.Height - 1));
            _world.Set<MouseResource>(new(
                mouseDiff,
                mouseDelta,
                scrollDelta,
                position,
                worldPosition,
                tilePosition,
                mouseState.LeftButton == ButtonState.Pressed,
                mouseState.MiddleButton == ButtonState.Pressed,
                mouseState.RightButton == ButtonState.Pressed)
            );

            _lastMouseState = mouseState;
        }
    }
}
