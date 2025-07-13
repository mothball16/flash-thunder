
using FlashThunder.GameLogic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FlashThunder.ECSGameLogic.Components;

[ComponentName("gridPosition")]
internal readonly struct GridPosition
{
    public int X { get; }
    public int Y { get; }

    public static implicit operator GridPosition(Point p) => new(p.X, p.Y);
    public GridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}
