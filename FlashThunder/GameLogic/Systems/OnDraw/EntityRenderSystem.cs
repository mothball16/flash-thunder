using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlashThunder.GameLogic.Systems.OnDraw;

internal sealed class EntityRenderSystem(World world) : IUpdateSystem<SpriteBatch>
{
    private const int t = GameConstants.TileSize;

    // queries
    private readonly Stream<SpriteData, GridPosition> _drawableQuery
        = world.Query<SpriteData, GridPosition>().Stream();

    public void Update(SpriteBatch sb)
    {
        _drawableQuery.For((ref SpriteData sprite, ref GridPosition pos) =>
        {
            foreach (var layer in sprite.Layers.Values)
            {
                sb.Draw(
                    texture: layer.Texture,
                    destinationRectangle: new Rectangle(pos.X * t, pos.Y * t, t, t),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0,
                    origin: default,
                    effects: SpriteEffects.None,
                    layerDepth: layer.ZIndex);
            }
        });
    }
    public void Dispose() { }
}
