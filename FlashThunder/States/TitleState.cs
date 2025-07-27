using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Managers;
using FlashThunder.Screens;
using FlashThunder.Screens.Management;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.States;

internal sealed class TitleState : IGameState
{
    private readonly ScreenManager _screenManager;
    public TitleState(ScreenManager screenManager)
    {
        _screenManager = screenManager;
    }
    public void Enter()
    {
        _screenManager.LoadTitleScreen();
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
