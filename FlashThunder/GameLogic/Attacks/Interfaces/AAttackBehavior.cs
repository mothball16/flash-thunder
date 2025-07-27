using fennecs;
using FlashThunder.GameLogic.Attacks.Data;
using FlashThunder.GameLogic.Attacks.Components;

namespace FlashThunder.GameLogic.Attacks.Interfaces;

public abstract class AAttackBehavior : IAttackBehavior
{
    public abstract AttackInstance Execute(World world, AttackData data);
    
    protected static void ReleaseAttackTag(AttackData data)
    {
        // Base implementation for releasing attack tags
        if (data.Attacker.Has<WantsToAttack>())
        {
            data.Attacker.Remove<WantsToAttack>();
        }
    }
}