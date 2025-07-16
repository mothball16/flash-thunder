using fennecs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using static System.Formats.Asn1.AsnWriter;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Utilities;
using FlashThunder.Extensions;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Resources;
using FlashThunder.GameLogic.Components;

namespace FlashThunder.GameLogic.Systems.OnUpdate
{
    internal sealed class CameraSystems(World world, Camera camera) : IUpdateSystem<float>
    {
        /// <summary>
        /// The Camera object that this system will control.
        /// </summary>
        private readonly Camera _physicalCamera = camera;
        private readonly Stream<Components.WorldCamera> _cameras
            = world.Query<Components.WorldCamera>().Stream();
        private readonly Stream<Components.WorldCamera> _selectedCamera
            = world.Query<Components.WorldCamera, ActiveCamera>().Stream();

        public void Update(float dt)
        {
            (_, Components.WorldCamera cam) = _selectedCamera.FirstOrDefault();
            CameraInputSystem(cam);
            CameraUpdateSystem(dt);
            CameraSyncSystem();
        }

        public void CameraInputSystem(WorldCamera cam)
        {
            var input = world.GetResource<InputResource>();
            var camSpeed = input.Active.Contains(GameAction.SpeedUpCamera) 
                ? 16
                : 4;

            if (input.Active.Contains(GameAction.MoveLeft))
                cam.Target += new Vector2(-camSpeed, 0);

            if (input.Active.Contains(GameAction.MoveRight))
                cam.Target += new Vector2(camSpeed, 0);

            if (input.Active.Contains(GameAction.MoveUp))
                cam.Target += new Vector2(0, -camSpeed);

            if (input.Active.Contains(GameAction.MoveDown))
                cam.Target += new Vector2(0, camSpeed);
        }

        public void CameraUpdateSystem(float dt)
        {
            _cameras.For((ref Components.WorldCamera cam) =>
            {
                var finalTarget = cam.Target + cam.Offset + cam.Jitter;
                // update the camera
                float rate = 1 - MathF.Exp(-dt * cam.Response);
                cam.Position = Vector2.Lerp(cam.Position, finalTarget, rate);
                // reset jitter
                cam.Jitter = Vector2.Zero;
            });
        }

        public void CameraSyncSystem()
        {
            (_, Components.WorldCamera cam) = _selectedCamera.FirstOrDefault();
            if (cam == null)
            {
                Logger.Error("No active camera!");
                return;
            }
            _physicalCamera.XY = cam.Position;
        }

        public void Dispose() { }
    }
}