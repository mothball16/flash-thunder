using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Events;

namespace FlashThunder.GameLogic.Turn.Systems;

/// <summary>
/// System that handles player input to end the current turn
/// </summary>
internal sealed class EndTurnInputSystem : AUpdateSystem<float>
{
    private readonly World _world;
    
    public EndTurnInputSystem(World world)
    {
        _world = world;
    }
    
    public override void Update(float dt)
    {
        // Get the input resource
        if (!_world.HasResource<InputResource>()) return;
        
        var inputResource = _world.GetResource<InputResource>();
        
        // Check if the player pressed the EndTurn action
        if (inputResource.Input.JustActivated.Contains(GameAction.EndTurn))
        {
            // Publish a NextTurnRequest event to trigger turn transition
            _world.Send(new NextTurnRequest());
        }
    }
}