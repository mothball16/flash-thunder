using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
using FlashThunder.Defs;

namespace FlashThunder.Gameplay.Systems.OnDraw
{
    /// <summary>
    /// Renders the tilemap. Done separately because it's handled differently than entity rendering.
    /// </summary>
    internal class TileMapRenderSystem : ISystem<SpriteBatch>
    {
        private readonly TileMapComponent _map;
        private readonly World _world;
        private readonly TileManager _tiles;
        private int _tileSize;
        public bool IsEnabled { get; set; }

        public TileMapRenderSystem(World world, TileManager tiles)
        {
            _world = world;
            _tiles = tiles;
            _map = world.Get<TileMapComponent>();
            _tileSize = world.Get<EnvironmentResource>().TileSize;
        }

        public void Update(SpriteBatch sb)
        {
            char[][] tileMap = _map.Map;
            for (int row = 0; row < tileMap.Length; row++)
            {
                for (int col = 0; col < tileMap[row].Length; col++)
                {
                    char rep = tileMap[row][col];
                    TileDef tile = _tiles[rep];
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