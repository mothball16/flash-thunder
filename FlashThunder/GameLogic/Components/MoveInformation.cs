using FlashThunder.Enums;
using FlashThunder.GameLogic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlashThunder.ECSGameLogic.Components;

internal struct MovableTiles
{
    public Dictionary<Point, List<Point>> Tiles { get; set; }
}

internal struct MoveIntent
{
    public Direction Dir { get; set; }
    public List<Point> Waypoints { get; set; }
}