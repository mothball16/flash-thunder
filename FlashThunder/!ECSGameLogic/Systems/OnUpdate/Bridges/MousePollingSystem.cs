using DefaultEcs;
using DefaultEcs.System;
using System;
using FlashThunder.Managers;
using FlashThunder.Enums;
using Microsoft.Xna.Framework.Input;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.ECSGameLogic.Resources;
using Microsoft.Xna.Framework;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges
{
    /// <summary>
    /// Mostly just updates the mouse position since it's less beneficial to use event-based to
    /// constantly update where it is
    /// </summary>
    internal sealed class MousePollingSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly InputManager<GameAction> _manager;
        private readonly Camera _camera;
        public bool IsEnabled { get; set; }
        public MousePollingSystem(World world, InputManager<GameAction> manager, Camera camera)
        {
            _world = world;
            _manager = manager;
            _camera = camera;

            // safety check -- has our mouse resource been made yet?
            if (!world.Has<MouseResource>())
                world.Set(new MouseResource());
        }

        /// <summary>
        /// Updates the mouse resource.
        /// </summary>
        /// <param name="_"></param>
        public void Update(float _)
        {
            // retrieve the tilesize
            var tileSize = _world.Get<EnvironmentResource>().TileSize;

            // pull out a bunch of mouse shit
            ref var mouse = ref _world.Get<MouseResource>();
            var mouseState = _manager.GetMouseState();
            var mousePos = _manager.GetMousePosition();
            var mouseDiff = mousePos - mouse.Position;
            var mouseDelta = Math.Sqrt((mouseDiff.X * mouseDiff.X) + (mouseDiff.Y * mouseDiff.Y));

            // poll the mouse state for button presses
            mouse.LPressed = mouseState.LeftButton == ButtonState.Pressed;
            mouse.MPressed = mouseState.MiddleButton == ButtonState.Pressed;
            mouse.RPressed = mouseState.RightButton == ButtonState.Pressed;

            // update mouse positions
            // (screen: mouse pixel on screen)
            // (world: mouse position from the world origin)
            // (tile: mouse position in world tile coordinates)
            mouse.Position = mousePos;
            mouse.WorldPosition = _camera.ScreenToWorld(mousePos);
            mouse.TilePosition = new Point(
                mouse.WorldPosition.X / tileSize, 
                mouse.WorldPosition.Y / tileSize);

            // update mouse deltas
            mouse.Diff = mouseDiff;
            mouse.Delta = (float) mouseDelta;
        }

        public void Dispose() { }
    }
}