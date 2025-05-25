using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;
namespace FlashThunder.Core.Systems
{
    internal class EntityRenderSystem : ISystem<SpriteBatch>
    {
        // - - - [ Private Fields ] - - -
        private readonly GameContext _context;
        private readonly EntitySet _entitySet;
        private int _pxPerTile;
        // - - - [ Properties ] - - -
        public bool IsEnabled { get; set; }

        public EntityRenderSystem(GameContext context, int pxPerTile)
        {
            _context = context;
            _pxPerTile = pxPerTile;
            _entitySet = context.World.GetEntities()
                .With<MapPosComponent>()
                .With<SpriteDataComponent>()
                .AsSet();
        }


        public void Update(SpriteBatch sb)
        {

            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {
                SpriteDataComponent spData = e.Get<SpriteDataComponent>();
                MapPosComponent pos = e.Get<MapPosComponent>();
                
                sb.Draw(
                    texture: spData.Texture,
                    destinationRectangle: new(
                        (int) (pos.X * _pxPerTile), 
                        (int) (pos.Y * _pxPerTile),
                        _pxPerTile,
                        _pxPerTile
                        ),
                    color: Color.White);
            }
        }
        public void Dispose() { }
    }
}
