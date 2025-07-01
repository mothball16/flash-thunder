using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder._ECSGameLogic.Misc
{
    public struct SpriteLayer
    {
        public Texture2D Texture { get; set; }

        // scale as 1, 1 means that the sprite will be contained within a 1 x 1 tile
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        /// <summary>
        /// higher = rendered on top of lower z-index sprites
        /// </summary>
        public int ZIndex { get; set; }
    }
}
