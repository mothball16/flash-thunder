using fennecs;
using FlashThunder.GameLogic.Components.Turn;
using FlashThunder.GameLogic.Attacks.Components;

namespace FlashThunder.GameLogic.Turn.Systems;

/// <summary>
/// System that validates attack actions within turn constraints
/// </summary>
internal sealed class TurnBasedAttackSystem : AUpdateSystem<float>
{
    private readonly World _world;
    
    public TurnBasedAttackSystem(World world)
    {
        _world = world;
    }
    
    public override void Update(float dt)
    {
        // Find entities that want to attack and validate against turn constraints
        var attackQuery = _world.Query<WantsToAttack, TurnRange>().Compile();
        
        attackQuery.For((Entity entity, ref WantsToAttack wantsToAttack, ref TurnRange turnRange) =>
        {
            // Check if the unit can still attack this turn
            if (!turnRange.CanAttack)
            {
                // Remove the attack request - unit cannot attack this turn
                entity.Remove<WantsToAttack>();
                return;
            }
            
            // Attack is valid, mark that this unit has used its action
            turnRange.UseAction();
        });
    }
}