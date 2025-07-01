using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Components;
using MonoGame.Shapes;
using System;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw
{
    /// <summary>
    /// Renders the tilemap. Done separately because it's handled differently than entity rendering.
    /// </summary>
    internal sealed class SelectedTileRenderSystem : ISystem<SpriteBatch>
    {
        private readonly World _world;
        public bool IsEnabled { get; set; }

        public SelectedTileRenderSystem(World world)
        {
            _world = world;
        }

        public void Update(SpriteBatch sb)
        {
            var envRes = _world.Get<EnvironmentResource>();
            var mouseRes = _world.Get<MouseResource>();
            sb.DrawRectangle(
                new Rectangle(
                    mouseRes.TileX * envRes.TileSize,
                    mouseRes.TileY * envRes.TileSize,
                    envRes.TileSize,
                    envRes.TileSize),
                new Color(255,200,200),
                2);
        }

        public void Dispose()
        {
        }
    }
}