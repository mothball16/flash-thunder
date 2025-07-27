using fennecs;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Team.Components;

namespace FlashThunder.GameLogic.Turn.Components;

/// <summary>
/// Component containing turn display information for the UI
/// </summary>
internal struct TurnDisplayInfo
{
    public string CurrentTeamName { get; set; }
    public int RoundNumber { get; set; }
    public bool IsPlayerTurn { get; set; }
    
    public TurnDisplayInfo()
    {
        CurrentTeamName = "Unknown";
        RoundNumber = 1;
        IsPlayerTurn = false;
    }
    
    public void UpdateFromTurnOrder(TurnOrderResource turnOrder)
    {
        RoundNumber = turnOrder.RoundNumber;
        
        if (turnOrder.Order.Count > 0)
        {
            var currentTeam = turnOrder.CurTeam;
            if (currentTeam.Has<TeamTag>())
            {
                CurrentTeamName = currentTeam.Ref<TeamTag>().Team;
            }
            
            // Check if it's the player's turn
            IsPlayerTurn = currentTeam.Has<IsPlayerControllable>();
        }
    }
}