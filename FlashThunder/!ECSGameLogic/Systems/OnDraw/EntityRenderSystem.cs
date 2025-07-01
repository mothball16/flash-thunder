using System;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw
{
    internal sealed class EntityRenderSystem : ISystem<SpriteBatch>
    {
        // - - - [ Private Fields ] - - -
        private readonly EntitySet _entitySet;
        private readonly int _tileSize;

        // - - - [ Properties ] - - -
        public bool IsEnabled { get; set; }

        public EntityRenderSystem(World world)
        {
            _tileSize = world.Get<EnvironmentResource>().TileSize;

            _entitySet = world.GetEntities()
                .With<GridPosComponent>()
                .With<SpriteDataComponent>()
                .AsSet();
        }

        public void Update(SpriteBatch sb)
        {
            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {
                var spData = e.Get<SpriteDataComponent>();
                var pos = e.Get<GridPosComponent>();

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