using System.Collections.Generic;

namespace FlashThunder.GameLogic.Actions.Components;

public struct SkillSet
{
    public List<UnitSkill> Skills { get; set; }

    public readonly UnitSkill this[int index]
    {
        get { return Skills[index]; }
    }
}
