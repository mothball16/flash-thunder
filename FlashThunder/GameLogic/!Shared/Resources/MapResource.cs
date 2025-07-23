namespace FlashThunder.GameLogic.Resources;

/// <summary>
/// Global component for rendering tiles.
/// Consider making a map entity instead -- this is just to make sure it works for now
/// </summary>
internal struct MapResource
{
    public char[][] Tiles { get; set; }
}
