using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Systems.OnDraw;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Debugging;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;
using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Screens;
using FlashThunder.Snapshots;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.States;

/// <summary>
/// Manages the flow of the game by calling the appropriate systems from the context
/// for updating and drawing.
/// </summary>
internal class GameRunningState : IGameState
{
    private readonly GameContext _context;
    private readonly EventBus _eventBus;
    private MouseSnapshot _lastMouseSnapshot;
    private GameFrameSnapshot _lastGameFrameSnapshot;
    public GameRunningState(
        EventBus eventBus,
        GameContext context)
    {
        _eventBus = eventBus;
        _context = context;
        _lastMouseSnapshot = new();
        _lastGameFrameSnapshot = new();
    }

    private GameFrameSnapshot CreateGameSnapshot(float dt)
    {
        var actionSnapshot = _context.CreateActionSnapshot();
        var mouseSnapshot = _context.CreateMouseSnapshot(_lastMouseSnapshot);
        return new(dt, _context.World, actionSnapshot, mouseSnapshot);
    }

    private DrawFrameSnapshot CreateDrawSnapshot(SpriteBatch sb)
    {
        // we shouldn't be recalculating the actions/mouse here
        var actionsSnapshot = _lastGameFrameSnapshot.Actions;
        var mouseSnapshot = _lastGameFrameSnapshot.Mouse;
        return new(sb, actionsSnapshot, mouseSnapshot);
    }

    public void Enter()
    {
        _eventBus.Publish<LoadScreenEvent>(new()
        {
            ScreenFactory = () => new GameScreen(_eventBus).Visual,
            Layer = ScreenLayer.Primary
        });
    }
    public void Exit()
    {
        _context?.Dispose();
    }
    public void Update(float dt)
    {
        var snapshot = CreateGameSnapshot(dt);
        _context.UpdateSystems.Update(snapshot);
        _lastGameFrameSnapshot = snapshot;
        _lastMouseSnapshot = snapshot.Mouse;
    }
    public void Draw(SpriteBatch sb)
    {
        var snapshot = CreateDrawSnapshot(sb);
        _context.DrawSystems.Update(snapshot);
    }
}
