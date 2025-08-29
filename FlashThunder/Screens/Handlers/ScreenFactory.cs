using fennecs;
using FlashThunder.Managers;
using Gum.Wireframe;

namespace FlashThunder.Screens.Handlers;

internal class ScreenFactory
{
    private readonly TextureManager _textureManager;
    private readonly EventBus _eventBus;
    public ScreenFactory(TextureManager textureManager, EventBus eventBus)
    {
        _textureManager = textureManager;
        _eventBus = eventBus;
    }
    public GraphicalUiElement CreateGameScreen(World world)
    {
        var view = new GameScreen();
        view.Presenter = new GameScreenPresenter(_textureManager, world, view, _eventBus);
        return view.Visual;
    }

    public GraphicalUiElement CreateTitleScreen()
    {
        var view = new TitleScreen();
        view.Presenter = new TitleScreenPresenter(view, _eventBus);
        return view.Visual;
    }
}
