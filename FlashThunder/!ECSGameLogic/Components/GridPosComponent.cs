
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FlashThunder.ECSGameLogic.Components;

internal struct GridPosComponent
{
    public int X { get; set; }
    public int Y { get; set; }

    public GridPosComponent(Point p) : this(p.X, p.Y) { }
    public GridPosComponent(int x, int y)
    {
        X = x;
        Y = y;
    }
}
