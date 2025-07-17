using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic;
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

namespace FlashThunder.GameLogic.Systems.OnDraw;

internal sealed class DecoratorSystems(
    World world,
    Texture2D selectedTexture)
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
    public void Dispose() { }
}
