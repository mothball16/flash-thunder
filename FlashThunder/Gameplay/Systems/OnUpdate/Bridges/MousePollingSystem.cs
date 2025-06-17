using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.Enums;
using FlashThunder.Gameplay.Events;
using Microsoft.Xna.Framework.Input;
using FlashThunder.Gameplay.Resources;
using Dcrew.MonoGame._2D_Camera;

namespace FlashThunder.Gameplay.Systems.OnUpdate.Bridges
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
            ref var mouse = ref _world.Get<MouseResource>();
            var mouseState = _manager.GetMouseState();
            var mousePos = _manager.GetMousePosition();
            var mouseDiff = (mousePos - mouse.Position);
            var mouseDelta = Math.Sqrt(mouseDiff.X * mouseDiff.X + mouseDiff.Y * mouseDiff.Y);

            mouse.LPressed = mouseState.LeftButton == ButtonState.Pressed;
            mouse.MPressed = mouseState.MiddleButton == ButtonState.Pressed;
            mouse.RPressed = mouseState.RightButton == ButtonState.Pressed;
            mouse.Position = mousePos;
            mouse.WorldPosition = _camera.ScreenToWorld(mousePos);

            mouse.Diff = mouseDiff;
            mouse.Delta = (float)mouseDelta;
        }

        public void Dispose() { }
    }
}