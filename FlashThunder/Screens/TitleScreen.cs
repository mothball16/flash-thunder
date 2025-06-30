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
        private readonly ITitleScreenPresenter _presenter;
        public TitleScreen(ITitleScreenPresenter presenter)
        {
            _presenter = presenter;
        }
        partial void CustomInitialize()
        {
            PlayButton.Click += (s, a) => _presenter.ToGame();
            ShopButton.Click += (s, a) => _presenter.ToShop();
        }


       

        public void Update(GameTime gameTime)
        {
            CautionLineTop.TextureLeft = (CautionLineTop.TextureLeft + 1) % 500;
            CautionLineBottom.TextureLeft = (CautionLineTop.TextureLeft - 1) % 500;
        }

    }
}
