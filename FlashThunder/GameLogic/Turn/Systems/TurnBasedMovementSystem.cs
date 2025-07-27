using fennecs;
using FlashThunder.GameLogic.Components.Turn;
using FlashThunder.GameLogic.Movement.Components;

namespace FlashThunder.GameLogic.Turn.Systems;

/// <summary>
/// System that validates and tracks movement actions within turn constraints
/// </summary>
internal sealed class TurnBasedMovementSystem : AUpdateSystem<float>
{
    private readonly World _world;
    
    public TurnBasedMovementSystem(World world)
    {
        _world = world;
    }
    
    public override void Update(float dt)
    {
        // Find entities that have a move intent and validate against turn constraints
        var moveQuery = _world.Query<MoveIntent, TurnRange>().Compile();
        
        moveQuery.For((Entity entity, ref MoveIntent moveIntent, ref TurnRange turnRange) =>
        {
            // If there are waypoints but the unit can't move, clear the intent
            if (moveIntent.Waypoints.Count > 0 && !turnRange.CanMove)
            {
                // Clear the move intent - unit cannot move this turn
                moveIntent.Waypoints.Clear();
                return;
            }
            
            // If movement just started (has waypoints and hasn't moved yet), mark as having used movement
            if (moveIntent.Waypoints.Count > 0 && !turnRange.HasMoved)
            {
                turnRange.UseMove();
            }
        });
    }
}