namespace FlashThunder.Core;

internal static class GameConstants
{
    public const int TileSize = 64;
    public const float MaxZoom = 8;
    public const float MinZoom = 0.5f;
    // The scroll value delta reqiured to go up one zoom level.
    public const float ScrollStep = 120 * 4;
}