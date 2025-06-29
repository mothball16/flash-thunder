using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.States;
using Microsoft.Xna.Framework;
using System;

namespace FlashThunder.Screens
{
    internal partial class TitleScreen : IUpdateScreen
    {
        partial void CustomInitialize()
        {
            PlayButton.Click += (s, a) => ToGame();
            ShopButton.Click += (s, a) => ToShop();

        }


        private static void ToGame()
        {
            EventBus.Publish<ChangeStateEvent>(new()
            {
                To = typeof(GameRunningState),
                From = typeof(MenuState)
            });
        }

        private static void ToShop()
        {
            EventBus.Publish<ChangeStateEvent>(new()
            {
                To = typeof(GameRunningState),
                From = typeof(MenuState)
            });
        }

        public void Update(GameTime gameTime)
        {
            //CautionLineTop.TextureLeft = (CautionLineTop.TextureLeft + 1) % 100;
            //CautionLineBottom.TextureLeft = (CautionLineTop.TextureLeft + 1) % 100;
        }

    }
}
