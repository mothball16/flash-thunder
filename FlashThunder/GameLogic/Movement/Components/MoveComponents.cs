using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Movement.Components;
/// <summary>
/// Cooldown until movement systems can act on the entity again. Mostly used for processing waypoints
/// in a controlled manner
/// </summary>
internal struct WaypointDebounce {
    public float Value { get; set; }
};

/// <summary>
/// Holds the tiles that an entity can move to, as well as their respective pathing waypoints.
/// This should not be re-calculated unless necessary (map condition changes, unit position changes)
/// 
/// </summary>
internal struct MovableTiles
{
    public Dictionary<Point, List<Point>> Tiles { get; set; }
}


internal struct MoveIntent
{
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

internal readonly record struct WorldToGridAnimator(float Response);
internal readonly record struct MoveInProgressTag;