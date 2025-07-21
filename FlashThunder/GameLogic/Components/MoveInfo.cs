using FlashThunder.Enums;
using FlashThunder.GameLogic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlashThunder.ECSGameLogic.Components;

internal struct MoveCD {
    public float Value { get; set; }
};

internal struct MovableTiles
{
    public Dictionary<Point, List<Point>> Tiles { get; set; }
}

internal struct MoveIntent
{
    public int X { get; set; }
    public int Y { get; set; }
    public List<Point> Waypoints { get; set; }

    public MoveIntent()
    {
        Waypoints = [];
    }
}
internal readonly record struct MoveCapable(
    int Range,
    string[] Traverse,
    float ProcessWaypointCD = 0.5f
    );