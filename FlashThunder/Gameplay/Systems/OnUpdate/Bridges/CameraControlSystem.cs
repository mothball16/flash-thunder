using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
using FlashThunder.Defs;
using Dcrew.MonoGame._2D_Camera;

namespace FlashThunder.Gameplay.Systems.OnUpdate.Bridges
{
    /// <summary>
    /// Updates the current camera.
    /// </summary>
    internal class CameraControlSystem : ISystem<float>
    {
        private World _world;
        private Camera _camera;
        public bool IsEnabled { get; set; }


        public CameraControlSystem(World world, Camera camera)
        {
            _world = world;
            _camera = camera;
        }

        private float NumLerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public void Update(float dt)
        {
            ref var camInfo = ref _world.Get<CameraResource>();

            //apply cam. translation from the current camera position
            var origCamPos = _camera.XY;
            origCamPos.X = NumLerp(origCamPos.X, camInfo.Target.X, camInfo.TweenFactor);
            origCamPos.Y = NumLerp(origCamPos.Y, camInfo.Target.Y, camInfo.TweenFactor);
            origCamPos += camInfo.Offset;
            _camera.XY = origCamPos;

            //reset offset (it is to be assigned frame-by-frame)
            camInfo.Offset = new Vector2(0,0);

            Console.WriteLine(camInfo.Target);
            //expose camera matrix for render system to act upon
            camInfo.TransformMatrix = _camera.View();
        }
        
        public void Dispose()
        {
        }
    }
}