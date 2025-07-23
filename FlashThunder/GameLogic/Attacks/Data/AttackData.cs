using fennecs;
using FlashThunder.GameLogic.AttackLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.AttackLogic.Data;

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
/// Represents the data required for a unit's skill for execution and display.
/// This is deserialized from the entity JSON.
/// </summary>
public struct UnitSkill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public int Cooldown { get; set; }
    public string AttackBehavior { get; set; }
    public IAttackParams AttackParams { get; set; }
}