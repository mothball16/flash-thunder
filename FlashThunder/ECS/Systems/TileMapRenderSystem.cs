using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECS.Resources;

namespace FlashThunder.ECS.Systems
{
    /// <summary>
    /// Renders the tilemap. Done separately because it's handled differently than entity rendering.
    /// </summary>
    internal class TileMapRenderSystem :ISystem<SpriteBatch>
    {
        private readonly TileMapComponent _map;
        private readonly World _world;
        private readonly AssetManager _assets;
        private int _tileSize;
        public bool IsEnabled { get; set; }
        public TileMapRenderSystem(World world, AssetManager assets) 
        {
            _world = world;
            _assets = assets;
            _map = world.Get<TileMapComponent>();
            _tileSize = world.Get<EnvironmentResource>().TileSize;
        }



        public void Update(SpriteBatch sb)
        {

            char[][] tileMap = _map.Map;
            for(int row = 0; row < tileMap.Length; row++)
            {
                for(int col = 0; col < tileMap[row].Length; col++)
                {
                    char rep = tileMap[row][col];
                    Texture2D tex = _assets.GetTex(rep);
                    sb.Draw(tex,
                        new Rectangle(col * _tileSize, row * _tileSize, _tileSize, _tileSize),
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
