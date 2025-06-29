using FlashThunder.Events;
using FlashThunder.Managers;
using FlashThunder.States;

namespace FlashThunder.Screens
{
    internal partial class TitleScreen
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
    }
}
