using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using Microsoft.Xna.Framework;

namespace FlashThunder.Gameplay.Resources
{
    /// <summary>
    /// Global component for updating the camera.
    /// </summary>
    public struct CameraResource
    {
        public Vector2 Target {  get; set; }
        public Vector2 Offset { get; set; }
        public Entity? Subject { get; set; }
        public float TweenFactor { get; set; }
        public float Scale { get; set; }
        public Matrix TransformMatrix { get; set; }


        public CameraResource()
        {
            Target = new(0, 0);
            Offset = new(0, 0);
            Subject = null;
            TweenFactor = 0.5f;
            Scale = 1;
            TransformMatrix = Matrix.Identity;
        }

    }
}
