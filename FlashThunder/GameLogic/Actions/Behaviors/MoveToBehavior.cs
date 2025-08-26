using fennecs;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.GameLogic.Actions.Interfaces;
using FlashThunder.GameLogic.Movement.Components;
using System;

namespace FlashThunder.GameLogic.Actions.Behaviors;

internal class MoveToBehavior : AAttackBehavior
{
    private static void CheckIfOver(ActionInstance instance, ActionData data)
    {
        if(data.Attacker.Ref<MoveIntent>().Waypoints.Count == 0)
        {
            instance.IsOver = true;
            ReleaseAttackTag(data);
        }
    }
    public override ActionInstance Execute(World world, ActionData data)
    {

        data.Attacker.Ref<MoveIntent>().Waypoints = data.Waypoints;

        return new ActionInstance()
        {
            Update = (instance, _) => CheckIfOver(instance, data),
        };
    }

}
