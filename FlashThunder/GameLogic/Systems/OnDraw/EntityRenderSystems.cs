using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlashThunder.GameLogic.Systems.OnDraw;

internal sealed class EntityRenderSystems : IUpdateSystem<SpriteBatch>
{
    private const int t = GameConstants.TileSize;
    private readonly Stream<SpriteData, GridPosition> _gridOnlyDrawableQuery;
    private readonly Stream<SpriteData, WorldPosition> _worldDrawableQuery;

    public EntityRenderSystems(World world)
    {
        _gridOnlyDrawableQuery = world.Query<SpriteData, GridPosition>()
            .Not<WorldPosition>()
            .Stream();
        _worldDrawableQuery = world.Query<SpriteData, WorldPosition>()
            .Stream();
    }

    private void GridPosOnlyEntityDrawSystem(SpriteBatch sb)
    {
        _gridOnlyDrawableQuery.For((ref SpriteData sprite, ref GridPosition pos) =>
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

    // World position should override grid position. World + grid should be paired with a
    // WorldToGridMover to indicate that the world position is meant to translate towards the grid
    // position. (Otherwise, there isn't a reason to use both.)
    private void WorldPosEntityDrawSystem(SpriteBatch sb)
    {
        _worldDrawableQuery.For((ref SpriteData sprite, ref WorldPosition pos) =>
        {
            Logger.Print($"Drawing entity at world position: {pos.X}, {pos.Y}");
            foreach (var layer in sprite.Layers.Values)
            {
                sb.Draw(
                    texture: layer.Texture,
                    destinationRectangle: new Rectangle((int) pos.X, (int) pos.Y, t, t),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0,
                    origin: default,
                    effects: SpriteEffects.None,
                    layerDepth: layer.ZIndex);
            }
        });
    }
    public void Update(SpriteBatch sb)
    {
        GridPosOnlyEntityDrawSystem(sb);
        WorldPosEntityDrawSystem(sb);
    }
    public void Dispose() { }
}
