namespace FlashThunder.ECSGameLogic.Components;

/// <summary>
/// Generally used if an entity is not bound to a grid. Like visual effects and stuff.
/// </summary>
internal struct WorldPosition
{
    public float X { get; set; }
    public float Y { get; set; }

    public WorldPosition(float x, float y)
    {
        X = x;
        Y = y;
    }
}
