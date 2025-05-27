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
namespace FlashThunder.Gameplay.Systems
{
    /// <summary>
    /// Mostly just updates the mouse position since it's less beneficial to use event-based to
    /// constantly update where it is
    /// 
    /// </summary>
    internal class MousePollingSystem :ISystem<float>
    {
        private World _world;
        private InputManager<PlayerAction> _manager;
        public bool IsEnabled { get; set; }
        public MousePollingSystem(World world, InputManager<PlayerAction> manager)
        {
            _world = world;
            _manager = manager;

            //safety check -- has our mouse resource been made yet?
            if (!world.Has<MouseResource>())
                world.Set(new MouseResource());
        }


        public void Update(float _)
        {
            ref var mouse = ref _world.Get<MouseResource>();
            mouse.Position = _manager.GetMousePosition();

            var mouseState = _manager.GetMouseState();
            mouse.LPressed = mouseState.LeftButton == ButtonState.Pressed;
            mouse.MPressed = mouseState.MiddleButton == ButtonState.Pressed;
            mouse.RPressed = mouseState.RightButton == ButtonState.Pressed;
        }

        public void Dispose() 
        { 
        }
    }
}
