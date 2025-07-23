using fennecs;
using FlashThunder.Core;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.GameLogic.Rendering.Systems;

internal sealed class DecoratorSystems
    : IUpdateSystem<SpriteBatch>
{
    private const string TileHoveringTexture = "tile_hovering_tile";
    private const string ControlledTileTexture = "controlled_tile";
    private const string CanMoveToTileTexture = "can_move_tile"; 
    private const string CanAttackTileTexture = "can_attack_tile";

    private const int t = GameConstants.TileSize;
    private readonly World _world;
    private readonly TextureManager _texManager;
    // queries
    private readonly Stream<GridPosition> _selectedDrawableQuery;
    private readonly Stream<MovableTiles> _movableTilesOfSelectedQuery;

    public DecoratorSystems(World world, TextureManager texManager)
    {
        _world = world;
        _texManager = texManager;
        _selectedDrawableQuery = world.Query<GridPosition>()
            .Has<SelectedTag>()
            .Stream();
        _movableTilesOfSelectedQuery = world.Query<MovableTiles>()
            .Has<SelectedTag>()
            .Stream();
    }

    public void Update(SpriteBatch sb)
    {
        SelectedDecoratorSystem(sb);
        HoveringTileDecoratorSystem(sb);
        MovableTilesDecoratorSystem(sb);
    }

    private void SelectedDecoratorSystem(SpriteBatch sb)
    {
        _selectedDrawableQuery.For((ref GridPosition pos) =>
        {
            sb.Draw(
            texture: _texManager.Get(ControlledTileTexture),
            destinationRectangle: new Rectangle(pos.X * t, pos.Y * t, t, t),
            sourceRectangle: null,
            color: Color.White,
            rotation: 0,
            origin: default,
            effects: SpriteEffects.None,
            layerDepth: 0);
        });
    }

    private void HoveringTileDecoratorSystem(SpriteBatch sb)
    {
        var mouse = _world.GetResource<MouseResource>();
        sb.Draw(
            texture: _texManager.Get(TileHoveringTexture),
            destinationRectangle: new Rectangle(mouse.TileX * t, mouse.TileY * t, t, t),
            sourceRectangle: null,
            color: Color.White,
            rotation: 0,
            origin: default,
            effects: SpriteEffects.None,
            layerDepth: 0);
    }

    private void MovableTilesDecoratorSystem(SpriteBatch sb)
    {
        _movableTilesOfSelectedQuery.For((ref MovableTiles movableTiles) =>
        {
            foreach (var tile in movableTiles.Tiles)
            {
                sb.Draw(
                    texture: _texManager.Get(CanMoveToTileTexture),
                    destinationRectangle: new Rectangle(tile.Key.X * t, tile.Key.Y * t, t, t),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0,
                    origin: default,
                    effects: SpriteEffects.None,
                    layerDepth: 0);
            }
        });
    }
    public void Dispose() { }
}
