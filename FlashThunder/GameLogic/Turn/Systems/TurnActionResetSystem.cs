using fennecs;
using FlashThunder.GameLogic.Components.Turn;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.GameLogic.Events;

namespace FlashThunder.GameLogic.Turn.Systems;

/// <summary>
/// System that resets turn actions for all units when a new turn begins
/// </summary>
internal sealed class TurnActionResetSystem : AUpdateSystem<float>
{
    private readonly World _world;
    private Entity? _lastCurrentTeam;
    
    public TurnActionResetSystem(World world)
    {
        _world = world;
    }
    
    public override void Update(float dt)
    {
        // Find the current team
        var currentTeamQuery = _world.Query<IsCurrentTurn>().Compile();
        var currentTeam = currentTeamQuery.FirstOrDefault();
        
        // If the current team has changed since last frame, reset turn actions
        if (currentTeam != default && currentTeam != _lastCurrentTeam)
        {
            ResetTurnActionsForTeam(currentTeam);
            _lastCurrentTeam = currentTeam;
        }
    }
    
    private void ResetTurnActionsForTeam(Entity currentTeam)
    {
        // Get the team tag to identify which units belong to this team
        if (!currentTeam.Has<TeamTag>()) return;
        
        var teamTag = currentTeam.Ref<TeamTag>().Team;
        
        // Find all units belonging to this team and reset their turn actions
        var unitQuery = _world.Query<TurnRange, TeamTag>().Compile();
        unitQuery.For((ref TurnRange turnRange, ref TeamTag unitTeam) =>
        {
            if (unitTeam.Team == teamTag)
            {
                turnRange.ResetForNewTurn();
            }
        });
    }
}