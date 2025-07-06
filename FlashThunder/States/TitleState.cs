using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Screens;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using System;

namespace FlashThunder.States;

internal class TitleState : IGameState, ITitleScreenPresenter
{
    private readonly UIElementFactory _uiFactory;
    private readonly EventBus _eventBus;
    public TitleState(EventBus eventBus)
    {
        _eventBus = eventBus;
        _uiFactory = () => new TitleScreen(this).Visual;
    }
    public void Enter()
    {
        // call the UImanager to load the view with its dependencies
        _eventBus.Publish<LoadScreenEvent>(new()
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

    public void ToGame()
    {
        _eventBus.Publish<ChangeStateEvent>(new()
        {
            To = typeof(GameRunningState),
            From = typeof(TitleState)
        });
    }

    public void ToShop()
    {
        _eventBus.Publish<ChangeStateEvent>(new()
        {
            To = typeof(GameRunningState),
            From = typeof(TitleState)
        });
    }
}
