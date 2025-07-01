using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.Enums;
using Microsoft.Xna.Framework;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input
{
    internal sealed class PlayerCameraInputSystem : ISystem<float>
    {
        private readonly World _world;
        public bool IsEnabled { get; set; }

        public PlayerCameraInputSystem(World world) => _world = world;

        public void Update(float dt)
        {
            var mouse = _world.Get<MouseResource>();
            var actionsHeld = _world.Get<ActionsResource>().Active;
            ref var camResource = ref _world.Get<CameraResource>();

            var camSpeed = actionsHeld.Contains(GameAction.SpeedUpCamera)
                ? 16
                : 4;

            if (actionsHeld.Contains(GameAction.MoveLeft))
            {
                camResource.Target += new Vector2(-camSpeed, 0);
            }

            if (actionsHeld.Contains(GameAction.MoveRight))
            {
                camResource.Target += new Vector2(camSpeed, 0);
            }

            if (actionsHeld.Contains(GameAction.MoveUp))
            {
                camResource.Target += new Vector2(0, -camSpeed);
            }

            if (actionsHeld.Contains(GameAction.MoveDown))
            {
                camResource.Target += new Vector2(0, camSpeed);
            }

            // Panning movement
            if (mouse.MPressed)
            {
                camResource.Target -= mouse.Diff.ToVector2();
            }
        }

        public void Dispose()
        {
        }
    }
}