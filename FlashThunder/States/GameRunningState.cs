
using fennecs;
using FlashThunder.Core;
using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Extensions;
using FlashThunder.Factories;
using FlashThunder.GameLogic;
using FlashThunder.Managers;
using FlashThunder.Screens;
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
    EventBus eventBus,
    List<IUpdateSystem<float>> updateSystems,
    List<IUpdateSystem<SpriteBatch>> drawSystems,
    List<IDisposable> disposables)
    : IGameState
{
    private readonly EventBus _eventBus = eventBus;
    private readonly List<IUpdateSystem<float>> _updateSystems = updateSystems;
    private readonly List<IUpdateSystem<SpriteBatch>> _drawSystems = drawSystems;
    private readonly List<IDisposable> _disposables = disposables;
    public void Enter()
    {
        _eventBus.Publish<LoadScreenEvent>(new()
        {
            ScreenFactory = () =>
            {
                var view = new GameScreen();
                view.Presenter = new GameScreenPresenter(world, view, _eventBus);
                return view.Visual;
            },
            Layer = ScreenLayer.Primary
        });
    }
    public void Update(float dt)
    {
        _updateSystems.ForEach(s => s.Update(dt));
    }
    public void Draw(SpriteBatch sb)
    {
        _drawSystems.ForEach(s => s.Update(sb));
    }

    public void Dispose()
    {
        world.Dispose();
        _disposables.ForEach(s => s.Dispose());
        _updateSystems.ForEach(s => s.Dispose());
        _drawSystems.ForEach(s => s.Dispose());
    }
}