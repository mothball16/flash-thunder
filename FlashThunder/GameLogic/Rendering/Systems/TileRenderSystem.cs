using fennecs;
using FlashThunder.Core;
using FlashThunder.GameLogic.Resources;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.GameLogic.Rendering.Systems
{
    internal sealed class TileRenderSystem(
        World world,
        TileManager tileManager)
        : IUpdateSystem<SpriteBatch>
    {
        private const int t = GameConstants.TileSize;

        // dependencies 
        private readonly TileManager _tileManager = tileManager;

        // queries
        private readonly MapResource _mapResource = world.GetResource<MapResource>();

        public void Update(SpriteBatch sb)
        {
            var tileMap = _mapResource.Tiles;
            for (int row = 0; row < tileMap.Length; row++)
            {
                for (int col = 0; col < tileMap[row].Length; col++)
                {
                    var rep = tileMap[row][col];
                    var tile = _tileManager[rep];
                    var tileBounds = new Rectangle(col * t, row * t, t, t);

                    sb.Draw(tile.Texture,tileBounds,Color.White);
                }
            }
        }

        public void Dispose() { }
    }
}
