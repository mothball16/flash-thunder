using FlashThunder.Events;
using FlashThunder.Managers;
using FlashThunder.States;
using Microsoft.Xna.Framework;

namespace FlashThunder.Screens;

internal partial class TitleScreen : IUpdateScreen
{
    public TitleScreenPresenter Presenter { get; set; }
    partial void CustomInitialize()
    {
        PlayButton.Click += (s, a) => Presenter.ToGame();
        ShopButton.Click += (s, a) => Presenter.ToShop();
    }

    public void Update(GameTime gameTime)
    {
        CautionLineTop.TextureLeft = (CautionLineTop.TextureLeft + 1) % 500;
        CautionLineBottom.TextureLeft = (CautionLineTop.TextureLeft - 1) % 500;
    }

}

internal sealed class TitleScreenPresenter
{
    private readonly TitleScreen _view;
    private readonly EventBus _bus;

    public TitleScreenPresenter(TitleScreen view, EventBus bus)
    {
        _view = view;
        _bus = bus;
    }

    public void ToGame()
    {
        _bus.Publish<ChangeStateEvent>(new()
        {
            To = typeof(GameRunningState),
            From = typeof(TitleState)
        });
    }

    public void ToShop()
    {
        _bus.Publish<ChangeStateEvent>(new()
        {
            To = typeof(GameRunningState),
            From = typeof(TitleState)
        });
    }
}