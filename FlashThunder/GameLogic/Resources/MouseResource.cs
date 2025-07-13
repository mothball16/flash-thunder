using Microsoft.Xna.Framework;

namespace FlashThunder.GameLogic.Resources;

public readonly record struct MouseResource(
    Point MouseDiff, // the point difference between the current and previous mouse position
    float MouseDelta, // the absolute difference between the current and previous mouse position
    float ScrollDelta, // the absolute difference between the current and previous scroll position

    Point Position, // the current mouse position in screen coordinates
    Point WorldPosition, // the current mouse position in world coordinates
    Point TilePosition, // the current mouse position in tile coordinates

    bool LPressed, // whether the left mouse button is pressed
    bool MPressed, // whether the middle mouse button is pressed
    bool RPressed // whether the right mouse button is pressed
    )
{
    public readonly int ScreenX
        => Position.X;
    public readonly int ScreenY
        => Position.Y;
    public readonly int WorldX
        => WorldPosition.X;
    public readonly int WorldY
        => WorldPosition.Y;
    public readonly int TileX
        => TilePosition.X;
    public readonly int TileY
        => TilePosition.Y;
}
