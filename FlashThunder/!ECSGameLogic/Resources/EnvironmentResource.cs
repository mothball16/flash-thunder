namespace FlashThunder.ECSGameLogic.Resources;

/// <summary>
/// Global component for game environment configurations.
/// </summary>
internal class EnvironmentResource
{
    // this is the list of teams in the match
    public string[] Teams { get; set; }
    // this is the team the systems render controls for
    public string FocusedTeam { get; set; }
}
