namespace FlashThunder.ECSGameLogic.Components;

/// <summary>
/// Global component for rendering tiles.
/// Consider making a map entity instead -- this is just to make sure it works for now
/// </summary>
internal class TileMapComponent
{
    public char[][] Map { get; set; }
}
