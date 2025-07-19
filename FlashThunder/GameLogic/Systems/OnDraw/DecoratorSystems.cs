using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Resources;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FlashThunder.GameLogic.Systems.OnDraw;

internal sealed class DecoratorSystems(
    World world,
    Texture2D selectedTexture,
    Texture2D hoveringTileTexture)
    : IUpdateSystem<SpriteBatch>
{
    private const int t = GameConstants.TileSize;

    // queries
    private readonly Stream<GridPosition> _drawableQuery
        = world.Query<GridPosition>()
        .Has<SelectedTag>()
        .Stream();

    public void Update(SpriteBatch sb)
    {
        SelectedDecoratorSystem(sb, selectedTexture);
        HoveringTileDecoratorSystem(sb, hoveringTileTexture);
    }

    private void SelectedDecoratorSystem(SpriteBatch sb, Texture2D tex)
    {
        _drawableQuery.For((ref GridPosition pos) =>
        {
            sb.Draw(
            texture: tex,
            destinationRectangle: new Rectangle(pos.X * t, pos.Y * t, t, t),
            sourceRectangle: null,
            color: Color.White,
            rotation: 0,
            origin: default,
            effects: SpriteEffects.None,
            layerDepth: 0);
        });
    }

    private void HoveringTileDecoratorSystem(SpriteBatch sb, Texture2D tex)
    {
        var mouse = world.GetResource<MouseResource>();
        sb.Draw(
            texture: tex,
            destinationRectangle: new Rectangle(mouse.TileX * t, mouse.TileY * t, t, t),
            sourceRectangle: null,
            color: Color.White,
            rotation: 0,
            origin: default,
            effects: SpriteEffects.None,
            layerDepth: 0);
    }
    public void Dispose() { }
}
