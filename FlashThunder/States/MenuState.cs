using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.States
{
    internal class MenuState : IGameState
    {
        public void Enter()
        {
            EventBus.Publish<LoadScreenEvent>(new()
            {
                To = typeof(Screens.TitleScreen),
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
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, 
                null, null, null);
        }
    }
}
