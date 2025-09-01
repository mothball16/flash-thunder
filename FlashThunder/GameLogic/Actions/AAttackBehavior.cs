using fennecs;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Actions.Data;

namespace FlashThunder.GameLogic.Actions;

public abstract class AAttackBehavior
{
    public abstract ActionInstance Execute(World world, ActionData data);

    // This ends the attack lock, allowing the unit to execute another attack.
    protected static void ReleaseAttackTag(ActionData data)
    {
        if (data.Attacker.Has<ExecutingActionTag>())
            data.Attacker.Remove<ExecutingActionTag>();
    }
}