using Microsoft.Xna.Framework;

namespace FlashThunder.GameLogic.Movement.Components;

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

internal struct GridPosition
{
    public int X { get; set; }
    public int Y { get; set; }

    public static implicit operator GridPosition(Point p) => new(p.X, p.Y);
    public GridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}