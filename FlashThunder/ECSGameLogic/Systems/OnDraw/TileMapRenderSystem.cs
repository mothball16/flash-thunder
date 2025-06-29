using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Components;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw
{
    /// <summary>
    /// Renders the tilemap. Done separately because it's handled differently than entity rendering.
    /// </summary>
    internal sealed class TileMapRenderSystem : ISystem<SpriteBatch>
    {
        private readonly TileMapComponent _map;
        private readonly TileManager _tiles;
        private readonly int _tileSize;
        public bool IsEnabled { get; set; }

        public TileMapRenderSystem(World world, TileManager tiles)
        {
            _tiles = tiles;
            _map = world.Get<TileMapComponent>();
            _tileSize = world.Get<EnvironmentResource>().TileSize;
        }

        public void Update(SpriteBatch sb)
        {
            var tileMap = _map.Map;

            for (int row = 0; row < tileMap.Length; row++)
            {
                for (int col = 0; col < tileMap[row].Length; col++)
                {
                    var rep = tileMap[row][col];
                    var tile = _tiles[rep];

                    var tileBounds = new Rectangle
                        (col * _tileSize, row * _tileSize, _tileSize, _tileSize);

                    sb.Draw(
                        tile.Texture,
                        tileBounds,
                        Color.White
                        );
                }
            }
        }

        public void Dispose()
        {
        }
    }
}