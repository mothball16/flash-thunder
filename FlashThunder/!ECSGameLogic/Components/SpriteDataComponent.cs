using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.ECSGameLogic.Components
{
    public class SpriteDataComponent(Texture2D texture, int x, int y)
    {
        public Texture2D Texture { get; set; } = texture;

        // scale as 1, 1 means that the sprite will be contained within a 1 x 1 tile
        public int SizeX { get; set; } = x;
        public int SizeY { get; set; } = y;
    }
}