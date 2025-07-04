﻿using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input
{
    /// <summary>
    /// Maintains a list of active actions on a given frame.
    /// </summary>
    internal sealed class ActionPollingSystem : ISystem<float>
    {
        private readonly World _world;
        public bool IsEnabled { get; set; }
        private readonly List<IDisposable> _subscriptions;

        public ActionPollingSystem(World world)
        {
            _world = world;

            _subscriptions = [
                world.Subscribe<ActionActivatedEvent>(AddAction),
                world.Subscribe<ActionReleasedEvent>(RemoveAction)
            ];

            // safety check -- has our mouse resource been made yet?
            if (!world.Has<ActionsResource>())
                world.Set(new ActionsResource());
        }
        private void AddAction(in ActionActivatedEvent msg)
        {
            ref var actions = ref _world.Get<ActionsResource>();
            actions.Active.Add(msg.Action);
        }

        private void RemoveAction(in ActionReleasedEvent msg)
        {
            ref var actions = ref _world.Get<ActionsResource>();
            actions.Active.Remove(msg.Action);
        }

        public void Update(float dt) { }

        public void Dispose() => _subscriptions?.ForEach(s => s.Dispose());
    }
}