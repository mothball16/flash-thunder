using fennecs;
using FlashThunder.Managers;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Screens.Handlers
{
    internal class ScreenFactory
    {
        private readonly EventBus _eventBus;
        public ScreenFactory(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public GraphicalUiElement CreateGameScreen(World world)
        {
            var view = new GameScreen();
            view.Presenter = new GameScreenPresenter(world, view, _eventBus);
            return view.Visual;
        }

        public GraphicalUiElement CreateTitleScreen()
        {
            var view = new TitleScreen();
            view.Presenter = new TitleScreenPresenter(view, _eventBus);
            return view.Visual;
        }
    }
}
