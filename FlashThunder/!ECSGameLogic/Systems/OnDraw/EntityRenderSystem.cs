using System;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw;

[With(typeof(GridPosComponent), typeof(SpriteDataComponent))]
internal sealed class EntityRenderSystem : AEntitySetSystem<DrawFrameSnapshot>
{
    // - - - [ Private Fields ] - - -
    private readonly int _tileSize;

    public EntityRenderSystem(World world) : base(world)
    {
        _tileSize = GameConstants.TileSize;
    }

    protected override void Update(DrawFrameSnapshot state, in Entity entity)
    {
        var spData = entity.Get<SpriteDataComponent>();
        var pos = entity.Get<GridPosComponent>();

        foreach (var layer in spData.Layers.Values)
        {
            state.SpriteBatch.Draw(
                texture: layer.Texture,
                destinationRectangle: new Rectangle(
                    pos.X * _tileSize,
                    pos.Y * _tileSize,
                    _tileSize,
                    _tileSize
                    ),
                color: Color.White
                //layerDepth: layer.ZIndex
                );
        }
        
    }
}