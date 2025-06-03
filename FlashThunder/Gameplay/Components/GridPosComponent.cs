using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Components
{
    public struct GridPosComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        public GridPosComponent(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
