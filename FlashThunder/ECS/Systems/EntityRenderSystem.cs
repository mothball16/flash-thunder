using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
using FlashThunder.ECS.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FlashThunder.ECS.Systems
{
    internal class EntityRenderSystem : ISystem<SpriteBatch>
    {
        // - - - [ Private Fields ] - - -
        private readonly World _world;
        private readonly EntitySet _entitySet;
        private int _tileSize;
        // - - - [ Properties ] - - -
        public bool IsEnabled { get; set; }

        public EntityRenderSystem(World world)
        {
            _world = world;
            _tileSize = _world.Get<EnvironmentResource>().TileSize;

            _entitySet = world.GetEntities()
                .With<MapPosComponent>()
                .With<SpriteDataComponent>()
                .AsSet();
        }


        public void Update(SpriteBatch sb)
        {
            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {
                var spData = e.Get<SpriteDataComponent>();
                var pos = e.Get<MapPosComponent>();
                
                sb.Draw(
                    texture: spData.Texture,
                    destinationRectangle: new(
                        pos.X * _tileSize, 
                        pos.Y * _tileSize,
                        _tileSize,
                        _tileSize
                        ),
                    color: Color.White);
            }
        }
        public void Dispose() { }
    }
}
