namespace FlashThunder.GameLogic.Resources;

/// <summary>
/// Global component for rendering tiles.
/// Consider making a map entity instead -- this is just to make sure it works for now
/// </summary>
internal struct MapResource
{
    public char[][] Tiles { get; set; }

    public readonly int Width
        => Tiles.Length > 0 ? Tiles[0].Length : 0;
    public readonly int Height
        => Tiles.Length;
}
