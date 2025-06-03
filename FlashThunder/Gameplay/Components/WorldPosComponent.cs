using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Components
{
    /// <summary>
    /// Generally used if an entity is not bound to a grid. Like visual effects and stuff.
    /// </summary>
    public struct WorldPosComponent
    {
        public float X { get; set; }
        public float Y { get; set; }

        public WorldPosComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
