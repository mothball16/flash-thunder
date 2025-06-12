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
    internal class PlayerCameraInputSystem : ISystem<float>
    {
        private readonly World _world;
        public bool IsEnabled { get; set; }
        private List<IDisposable> _subscriptions;

        public PlayerCameraInputSystem(World world)
        {
            _world = world;
            _subscriptions = [
                world.Subscribe<ActionActivatedEvent>(UpdateMovement),
                world.Subscribe<ActionReleasedEvent>()

            ];
        }

        private void UpdateMovement(in ActionActivatedEvent msg)
        {
            ref var cam = ref _world.Get<CameraResource>();
            switch (msg.action)
            {
                case Enums.PlayerAction.MoveLeft:
                    cam.Target = cam.Target + new Vector2(0, -10);
                    break;
                case Enums.PlayerAction.MoveRight:
                    cam.Target += new Vector2(0, 1);
                    break;
            }
        }

        public void Update(float dt)
        {
        }
        public void Dispose() 
        { 
            _subscriptions?.ForEach(s => s.Dispose());
        }
    }
}
