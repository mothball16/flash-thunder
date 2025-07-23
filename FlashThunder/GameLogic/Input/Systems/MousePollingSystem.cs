using Dcrew.MonoGame._2D_Camera;
using fennecs;
using FlashThunder.Core;
using FlashThunder.Extensions;
using FlashThunder.GameLogic.Input.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Input.Systems
{
    internal sealed class MousePollingSystem(World world, Camera camera) : IUpdateSystem<float>
    {
        private const int TileSize = GameConstants.TileSize;
        private readonly World _world = world;
        private readonly Camera _camera = camera;
        private MouseState _lastMouseState = Mouse.GetState();
        public void Dispose()
        {
            
        }

        public void Update(float upd)
        {
            var mouseState = Mouse.GetState();
            var position = mouseState.Position;

            var mouseDiff = position - _lastMouseState.Position;
            var mouseDelta = (float) Math.Sqrt(
                mouseDiff.X * mouseDiff.X +
                mouseDiff.Y * mouseDiff.Y);
            float scrollDelta = mouseState.ScrollWheelValue - _lastMouseState.ScrollWheelValue;

            var worldPosition = _camera.ScreenToWorld(position);
            var tilePosition = new Point(worldPosition.X / TileSize, worldPosition.Y / TileSize);
            _world.SetResource<MouseResource>(new(
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
