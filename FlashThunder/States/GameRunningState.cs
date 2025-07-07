using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Events;
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
using System.Collections.Generic;

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
    private readonly Queue<(GameAction action, bool activated)> _actionQueue;
    public GameRunningState(
        EventBus eventBus,
        GameContext context)
    {
        _eventBus = eventBus;
        _context = context;
        _lastMouseSnapshot = new();
        _lastGameFrameSnapshot = new();
        _actionQueue = new();

        context.InputManager.OnReleased += OnActionReleased;
        context.InputManager.OnActivated += OnActionActivated;
        context.InputManager.OnMouseScrolled += OnMouseScrolled;
    }

    public void OnActionActivated(GameAction action)
    {
        _actionQueue.Enqueue((action, true));
    }

    public void OnActionReleased(GameAction action)
    {
        _actionQueue.Enqueue((action, false));
    }

    public void OnMouseScrolled(int diff)
    {
        _context.World.Publish<MouseScrolledEvent>(new(diff));
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
        _context.InputManager.OnReleased -= OnActionReleased;
        _context.InputManager.OnActivated -= OnActionActivated;
        _context.InputManager.OnMouseScrolled -= OnMouseScrolled;
        _context.Dispose();
    }
    public void Update(float dt)
    {
        // create the snapshot before doing anything else
        var snapshot = CreateGameSnapshot(dt);

        // publish waiting events once we have the snapshot, in order of when they were requested
        while(_actionQueue.Count > 0)
        {
            var (action, activated) = _actionQueue.Dequeue();
            if (activated)
            {
                _context.World.Publish<ActionActivatedEvent>(new(action, snapshot.Mouse));
            }
            else
            {
                _context.World.Publish<ActionReleasedEvent>(new(action, snapshot.Mouse));
            }
        }

        // run the systems
        _context.UpdateSystems.Update(snapshot);

        // save this for comparison next update cycle
        _lastGameFrameSnapshot = snapshot;
        _lastMouseSnapshot = snapshot.Mouse;
    }
    public void Draw(SpriteBatch sb)
    {
        var snapshot = CreateDrawSnapshot(sb);
        _context.DrawSystems.Update(snapshot);
    }
}
