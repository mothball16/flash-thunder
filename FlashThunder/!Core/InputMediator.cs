using System;
using DefaultEcs;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.Enums;
using FlashThunder.Managers;

namespace FlashThunder.Core;

/// <summary>
/// Translates input manager interaction into ECS events.
/// This is initialized with GameContext and disposed with GameContext.
/// </summary>
internal sealed class InputMediator : IDisposable
{
    private readonly World _world;
    private readonly InputManager<GameAction> _manager;

    public InputMediator(World world, InputManager<GameAction> manager)
    {
        _world = world;
        _manager = manager;
        manager.OnReleased += OnActionReleased;
        manager.OnActivated += OnActionActivated;
    }

    public void OnActionActivated(GameAction action)
    {
        _world.Publish<ActionActivatedEvent>(new(action));
    }

    public void OnActionReleased(GameAction action)
    {
        _world.Publish<ActionReleasedEvent>(new(action));
    }

    /// <summary>
    /// Disconnects the events.
    /// </summary>
    public void Dispose()
    {
        if (_manager == null) return;
        _manager.OnActivated -= OnActionActivated;
        _manager.OnReleased -= OnActionReleased;

        GC.SuppressFinalize(this);
    }
}