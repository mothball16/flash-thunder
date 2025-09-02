using FlashThunder.GameLogic.Actions.Data;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Actions.Components;

public struct SkillEntry
{
    public UnitSkill Data { get; set; }
    public UnitSkillState State { get; set; }

    // consider changing this to a stupid bool that a system sets rather than this for better
    // ECS purity
    public readonly bool IsValid =>
        State.CanUse
        && State.UsesLeftThisTurn > 0
        && State.TurnsSinceLastUse >= Data.CooldownBetweenTurns;
}

public struct SkillSet
{
    public List<SkillEntry> Skills { get; set; }
    public List<UnitSkillState> SkillRuntimes { get; set; }
    public readonly SkillEntry this[int index]
    {
        get { return Skills[index]; }
    }
}
