using FlashThunder.GameLogic.Actions.Data;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Actions.Components;

internal struct AttackQueued
{
    public Queue<ActionData> Queue { get; set; }

    public AttackQueued()
    {
        Queue = [];
    }
}
