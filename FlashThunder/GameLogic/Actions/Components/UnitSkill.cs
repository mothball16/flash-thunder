using FlashThunder.Enums;
using FlashThunder.GameLogic.Actions.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Actions.Components;

/// <summary>
/// Represents the data required for a unit's skill for execution and display.
/// This is deserialized from the entity JSON and typically placed inside the SkillSet component.
/// </summary>
public struct UnitSkill
{
    // disp. stats
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Description { get; set; }
    // phys. stats
    public int Range { get; set; }
    public string[] Traverse { get; set; }
    public int Cooldown { get; set; }
    public string AttackBehavior { get; set; }
    public IAttackParams AttackParams { get; set; }
    public SelectionType SelectionType { get; set; }

    // deser. data
    [JsonIgnore]
    public Texture2D IconTexture { get; set; }
}