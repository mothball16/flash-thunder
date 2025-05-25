using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FlashThunder.ECS.Resources
{
    /// <summary>
    /// Global component for retrieving the mouse information during a frame.
    /// </summary>
    public struct MouseResource
    {
        public Point Position { get; set; }
        public bool LPressed { get; set; }
        public bool MPressed { get; set; }
        public bool RPressed { get; set; }
        public readonly int X 
            => Position.X;
        public readonly int Y 
            => Position.Y;

    }
}
