using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nkast.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework.Graphics;
namespace FlashThunder.Gameplay.Components
{
    public class SpriteDataComponent(Texture2D texture, Vector2 scale, Vector2? anchor = null)
    {
        public Texture2D Texture { get; set; } = texture;
        //scale as 1, 1 means that the sprite will be contained within a 1 x 1 tile
        public Vector2 Scale { get; set; } = scale;
        public Vector2 Anchor { get; set; } = anchor ?? new (scale.X/2, scale.Y/2);
        
    }
}
