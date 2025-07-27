using fennecs;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Turn.Components;

namespace FlashThunder.GameLogic.Turn.Systems;

/// <summary>
/// System that updates turn display information for the UI
/// </summary>
internal sealed class TurnDisplayUpdateSystem : AUpdateSystem<float>
{
    private readonly World _world;
    
    public TurnDisplayUpdateSystem(World world)
    {
        _world = world;
    }
    
    public override void Update(float dt)
    {
        // Get the turn order resource
        if (!_world.HasResource<TurnOrderResource>()) return;
        
        var turnOrder = _world.GetResource<TurnOrderResource>();
        
        // Find or create the turn display info resource
        if (!_world.HasResource<TurnDisplayInfo>())
        {
            _world.SetResource(new TurnDisplayInfo());
        }
        
        // Update the display info
        ref var displayInfo = ref _world.GetResource<TurnDisplayInfo>();
        displayInfo.UpdateFromTurnOrder(turnOrder);
    }
}