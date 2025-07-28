using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Attacks.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlashThunder.GameLogic.Attacks.Data;

/// <summary>
/// Represents the data required for the AttackExecutionSystem to process an attack.
/// </summary>
public readonly record struct AttackData(
    Entity Attacker,
    List<Entity> Opps,
    string Behavior,
    IAttackParams Params
);

/// <summary>
/// Represents an instance of an attack that is currently in progress. AttackManager ticks this.
/// </summary>
public class AttackInstance
{
    public Action<float> Update { get; set; }
    public bool IsOver { get; set; }
}

/// <summary>
/// Represents the data required for a unit's skill for execution and display.
/// This is deserialized from the entity JSON and typically placed inside the SkillSet component.
/// </summary>
public struct UnitSkill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public int Cooldown { get; set; }
    public string AttackBehavior { get; set; }
    public IAttackParams AttackParams { get; set; }
    [JsonIgnore]
    public Texture2D IconTexture { get; set; }
}