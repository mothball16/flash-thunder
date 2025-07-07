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
    private readonly int t = GameConstants.TileSize;

    public EntityRenderSystem(World world) : base(world)
    {
    }

    protected override void Update(DrawFrameSnapshot state, in Entity entity)
    {
        var spData = entity.Get<SpriteDataComponent>();
        var pos = entity.Get<GridPosComponent>();

        foreach (var layer in spData.Layers.Values)
        {
            state.SpriteBatch.Draw(
                texture: layer.Texture,
                destinationRectangle: new Rectangle(pos.X * t, pos.Y * t, t, t),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: default,
                effects: SpriteEffects.None,
                layerDepth: layer.ZIndex);
        }
        
    }
}