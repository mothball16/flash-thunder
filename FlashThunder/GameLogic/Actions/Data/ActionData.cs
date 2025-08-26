using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Actions.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FlashThunder.GameLogic.Actions.Data;

/// <summary>
/// Represents the data required for the ActionExecutionSystem to process an attack.
/// </summary>
public readonly record struct ActionData(
    Entity Attacker,
    string Behavior,
    IAttackParams Params,
    List<Point> Waypoints
)
{
    public readonly Point Target => Waypoints[^1];
}

/// <summary>
/// Represents an instance of an attack that is currently in progress. AttackManager ticks this.
/// </summary>
public class ActionInstance
{
    public Action<ActionInstance> OnStart { get; set; }
    public Action<ActionInstance, float> Update { get; set; }
    public Action<ActionInstance> OnEnd { get; set; }
    public bool IsOver { get; set; }
}
