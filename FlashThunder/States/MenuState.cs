using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Screens;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using System;

namespace FlashThunder.States
{
    internal class MenuState : IGameState
    {
        private readonly UIElementFactory _uiFactory;

        public MenuState()
        {
            _uiFactory = () => new TitleScreen().Visual;
        }
        public void Enter()
        {
            EventBus.Publish<LoadScreenEvent>(new()
            {
                ScreenFactory = _uiFactory,
                Layer = ScreenLayer.Primary
            });
        }
        public void Exit()
        {
            
        }
        public void Update(float dt)
        {
           
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
        }
    }
}
