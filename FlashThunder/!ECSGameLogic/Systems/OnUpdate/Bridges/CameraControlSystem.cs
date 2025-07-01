using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges
{
    /// <summary>
    /// Updates the physical camera based off of the data from the camera resource.
    /// This should run at the END of the update cycle to make sure every change has been applied
    /// on that frame.
    /// </summary>
    internal sealed class CameraControlSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly Camera _camera;
        public bool IsEnabled { get; set; }

        public CameraControlSystem(World world, Camera camera)
        {
            _world = world;
            _camera = camera;

            // safety check -- has our cam resource been made yet?
            if (!world.Has<CameraResource>())
                world.Set(new CameraResource());
        }

        private static float NumLerp(float a, float b, float t) => a + (b - a) * t;

        public void Update(float dt)
        {
            ref var camInfo = ref _world.Get<CameraResource>();

            // apply cam. translation from the current camera position
            var origCamPos = _camera.XY;
            origCamPos.X = NumLerp(origCamPos.X, camInfo.Target.X, camInfo.TweenFactor);
            origCamPos.Y = NumLerp(origCamPos.Y, camInfo.Target.Y, camInfo.TweenFactor);
            origCamPos += camInfo.Offset;
            _camera.XY = origCamPos;

            // reset offset (it is to be assigned frame-by-frame)
            camInfo.Offset = new Vector2(0, 0);

            // expose camera matrix for render system to act upon
            camInfo.TransformMatrix = _camera.View(-0);
        }

        public void Dispose()
        {
        }
    }
}