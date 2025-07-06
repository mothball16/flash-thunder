using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Components;
using MonoGame.Shapes;
using System;
using FlashThunder._ECSGameLogic;
using FlashThunder.Core;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw;

/// <summary>
/// Renders the tilemap. Done separately because it's handled differently than entity rendering.
/// </summary>
internal sealed class SelectedTileRenderSystem : ISystem<DrawFrameSnapshot>
{
    private readonly World _world;
    private const int t = GameConstants.TileSize;
    public bool IsEnabled { get; set; }

    public SelectedTileRenderSystem(World world)
    {
        _world = world;
    }

    public void Update(DrawFrameSnapshot state)
    {

        state.SpriteBatch.DrawRectangle(
            new Rectangle(state.Mouse.TileX * t, state.Mouse.TileY * t, t, t),
            new Color(255,200,200),
            2);
    }

    public void Dispose()
    {
    }
}