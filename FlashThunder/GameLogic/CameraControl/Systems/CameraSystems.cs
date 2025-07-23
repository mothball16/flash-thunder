using fennecs;
using Microsoft.Xna.Framework;
using System;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Components;
using FlashThunder.Core;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.GameLogic.CameraControl.Components;
using FlashThunder.GameLogic.CameraControl.Requests;

namespace FlashThunder.GameLogic.CameraControl.Systems
{
    internal sealed class CameraSystems : IUpdateSystem<float>
    {
        private readonly World _world;
        private readonly Camera _physicalCamera;
        private readonly Stream<WorldCamera> _cameras;
        private readonly Stream<WorldCamera> _selectedCamera;
        private readonly Stream<WorldCamera, SmoothScalable> _scalableSelectedCamera;
        private readonly Query _selectedIsMovable;

        public CameraSystems(World world, Camera camera)
        {
            _world = world;
            _physicalCamera = camera;

            // init the queries
            var baseQuery = world.Query<WorldCamera>();
            _cameras = baseQuery
                .Stream();
            _selectedCamera = baseQuery.Has<ActiveCamera>()
                .Stream();
            _scalableSelectedCamera = world.Query<WorldCamera, SmoothScalable>()
                .Has<IsPlayerControllable>()
                .Has<ActiveCamera>()
                .Stream();
            _selectedIsMovable = baseQuery
                .Has<IsPlayerControllable>()
                .Compile();
        }
        private static float NumLerp(float a, float b, float t)
            => a + (b - a) * t;
        public void Update(float dt)
        {
            CameraInputSystem(dt);
            CameraZoomSystem(dt);
            CameraUpdateSystem(dt);
            CameraSyncSystem();
        }

        public void CameraInputSystem(float dt)
        {
            if(_selectedIsMovable.IsEmpty)
                return;

            var input = _world.GetResource<InputResource>();
            var camSpeed = input.IsActivated(GameAction.SpeedUpCamera)
                ? 800
                : 200;

            if (input.IsActivated(GameAction.MoveLeft))
                _world.Publish(new CamTranslationRequest(-camSpeed * dt, 0));

            if (input.IsActivated(GameAction.MoveRight))
                _world.Publish(new CamTranslationRequest(camSpeed * dt, 0));

            if (input.IsActivated(GameAction.MoveUp))
                _world.Publish(new CamTranslationRequest(0, -camSpeed * dt));

            if (input.IsActivated(GameAction.MoveDown))
                _world.Publish(new CamTranslationRequest(0, camSpeed * dt));
        }

        public void CameraZoomSystem(float dt)
        {
            var mouse = _world.GetResource<MouseResource>();
            _scalableSelectedCamera.For((in Entity e, ref WorldCamera cam, ref SmoothScalable scale) =>
            {
                scale.ScaleTarget += mouse.ScrollDelta / GameConstants.ScrollStep;
                scale.ScaleTarget = Math.Clamp(scale.ScaleTarget, GameConstants.MinZoom, GameConstants.MaxZoom);

            });
        }
        public void CameraUpdateSystem(float dt)
        {
            // positional update
            _cameras.For((ref WorldCamera cam) =>
            {
                var finalTarget = cam.Target + cam.Offset + cam.Jitter;
                // update the camera
                float rate = 1 - MathF.Exp(-dt * cam.Response);
                cam.Position = Vector2.Lerp(cam.Position, finalTarget, rate);
                // reset jitter
                cam.Jitter = Vector2.Zero;
            });

            // scale update
            _scalableSelectedCamera.For((ref WorldCamera _, ref SmoothScalable scale) =>
            {
                float rate = 1 - MathF.Exp(-dt * scale.ScaleResponse);
                scale.ScaleCurrent = NumLerp(scale.ScaleCurrent, scale.ScaleTarget,rate);
                
            });
        }

        public void CameraSyncSystem()
        {
            _selectedCamera.For((ref WorldCamera cam) =>
            {
                _physicalCamera.XY = cam.Position;
            });

            _scalableSelectedCamera.For((ref WorldCamera cam, ref SmoothScalable scale) =>
            {
                _physicalCamera.Scale = new(scale.ScaleCurrent, scale.ScaleCurrent);
            });
        }

        public void Dispose() { }
    }
}