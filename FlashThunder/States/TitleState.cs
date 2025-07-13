using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Managers;
using FlashThunder.Screens;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using System;

namespace FlashThunder.States;

internal sealed class TitleState : IGameState
{
    private readonly EventBus _eventBus;
    public TitleState(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public void Enter()
    {
        // call the UImanager to load the view with its dependencies
        _eventBus.Publish<LoadScreenEvent>(new()
        {
            ScreenFactory = () => {
                var view = new TitleScreen();
                view.Presenter = new TitleScreenPresenter(view, _eventBus);
                return view.Visual;
            },
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

    public void Dispose()
    {
        // nothing to dispose
    }
}
