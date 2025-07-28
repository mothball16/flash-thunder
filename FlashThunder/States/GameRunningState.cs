
using fennecs;
using FlashThunder.GameLogic;
using FlashThunder.Screens.Handlers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FlashThunder.States;

/// <summary>
/// Manages the flow of the game by calling the appropriate systems from the context
/// for updating and drawing.
/// </summary>
internal sealed class GameRunningState(
    World world,
    ScreenManager screenManager,
    List<AUpdateSystem<float>> updateSystems,
    List<AUpdateSystem<SpriteBatch>> drawSystems,
    List<AUpdateSystem<float>> postCycleSystems,
    List<IDisposable> disposables)
    : IGameState
{
    private readonly ScreenManager _screenManager = screenManager;
    private readonly List<AUpdateSystem<float>> _updateSystems = updateSystems;
    private readonly List<AUpdateSystem<SpriteBatch>> _drawSystems = drawSystems;
    private readonly List<AUpdateSystem<float>> _postCycleSystems = postCycleSystems;
    private readonly List<IDisposable> _disposables = disposables;
    public void Enter()
    {
        _screenManager.LoadGameScreen(world);
    }
    public void Update(float dt)
    {
        _updateSystems.ForEach(s => s.Update(dt));
    }
    public void Draw(SpriteBatch sb)
    {
        _drawSystems.ForEach(s => s.Update(sb));
        _postCycleSystems.ForEach(s => s.Update(0f));
    }


    public void Dispose()
    {
        world.Dispose();
        _disposables.ForEach(s => s.Dispose());
        _updateSystems.ForEach(s => s.Dispose());
        _drawSystems.ForEach(s => s.Dispose());
        _postCycleSystems.ForEach(s => s.Dispose());
    }
}