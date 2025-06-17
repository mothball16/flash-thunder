using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Events;
using FlashThunder.Gameplay.Resources;
using Microsoft.Xna.Framework;
namespace FlashThunder.Gameplay.Systems.OnUpdate.Input
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
            actions.Active.Remove(msg.action);
        }

        public void Update(float dt) { }
        public void Dispose()
        {
            _subscriptions?.ForEach(s => s.Dispose());
        }
    }
}
